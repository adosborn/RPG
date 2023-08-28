using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public int moveSpeed = 100;
    bool dir = true;
    Vector3 newHitNormal;
    public Transform newHitPrefab;
    public GameObject hitDamgePrefab;
    int myDamage;
    Player player;
    // Update is called once per frame
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myDamage = player.stats.curAttack;
        newHitNormal = Weapon.hitNormal;
        dir = UnityStandardAssets._2D.PlatformerCharacter2D.m_FacingRight;
        if (dir) this.transform.localScale = new Vector3(1, 1, 1);
        else this.transform.localScale = new Vector3(-1, 1, 1);
    }
    void Update()
    {
        if (dir)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        Destroy(this.gameObject, 3);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.name != "Player")&& (col.name != "Player(Clone)"))
        {
            //Debug.Log("Hit " + col.name);
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(player.stats.curAttack);
            }
            if (newHitNormal != new Vector3(9999, 9999, 9999))
            {
                Transform hitParticle = Instantiate(newHitPrefab, this.transform.position, Quaternion.FromToRotation(Vector3.right, newHitNormal)) as Transform;
                Destroy(hitParticle.gameObject, 1);
            }
            Destroy(this.gameObject);
        }
    }
}
