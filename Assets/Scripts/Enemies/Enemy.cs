using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();
    public bool isMelee;
    public GameMaster gm; 
    public Transform coinPrefab;
    public LayerMask attackMask;
    public float attackRange = 1f;
    public float rarityValue = 0f;
    ItemSpawner itemSpawner;
    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    public float difficultyModifier; 

    void Start()
    {
        itemSpawner = ItemSpawner.instance;
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        difficultyModifier = gm.enemyDifficultyModifier;
        //Debug.Log("modifier: " + difficultyModifier + " / " + gm.enemyDifficultyModifier);
        stats.Init();
        ModifyStats(difficultyModifier);
        rarityValue += (difficultyModifier - 1);
        Debug.Log("Rarity: " + rarityValue);
        if (statusIndicator != null){
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void Update() {
        if (transform.position.y <= -20){
            Destroy(gameObject);
        }
    }

    public void ModifyStats(float modifier){
        float newMax = (float)this.stats.maxHealth * (0.5f + 0.5f * (modifier / 2));
        this.stats.maxHealth = (int)newMax;
        this.stats.curHealth = this.stats.maxHealth;
        float newDmg = (float)this.stats.damage * (0.5f + 0.5f * (modifier / 2));
        this.stats.damage = (int)newDmg;
    }
    
    public void DamageEnemy(int dmg){
        Player player = gm.player;
        player.StartDamageNum(dmg, transform.position, transform.rotation);
        stats.curHealth -= dmg;
        if (stats.curHealth <= 0)
        {
            player.GainXP((int)stats.maxHealth/3);
            if (isMelee) {
                StartCoroutine(DeathAnim());
            }
            else {
                GameMaster.KillEnemy(this, coinPrefab);
                itemSpawner.SpawnRandom(rarityValue, transform.position);
            }
            player.stats.curScore += (3+(int)this.rarityValue);
            //Debug.Log("KILLED ENEMY");
        }
        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    public void MeleeAttackPlayer(){
        Collider2D colInfo = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
        //Debug.Log(colInfo);
        if (colInfo != null) {
            //Debug.Log("SwordHit!!!!");
            Player _player = colInfo.GetComponent<Player>();
            _player.DamagePlayer((int)(stats.damage * (10.0f / _player.stats.curDefence)), colInfo.ClosestPoint(_player.transform.position));
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo){
        if (!isMelee) {
            Player _player = _colInfo.collider.GetComponent<Player>();
            if (_player != null){
                //Debug.Log(_player.stats.curDefence);
                //Debug.Log("Damgaging player " + stats.damage + " x " + (10.0f / _player.stats.curDefence));
                _player.DamagePlayer((int)(stats.damage * (10.0f / _player.stats.curDefence)), _colInfo.collider.ClosestPoint(_player.transform.position));
            }
        } 
    }

    IEnumerator DeathAnim() {
        GetComponent<Animator>().SetTrigger("Destroy");
        yield return new WaitForSeconds(0.43f);
        GameMaster.KillEnemy(this, coinPrefab);
        itemSpawner.SpawnRandom(rarityValue, transform.position);
    }
}
