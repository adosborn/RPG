using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityEngine.CoreModule;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int Damage = 10;
    //public bool facingRight = true;
    public LayerMask toHit;
    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;
    Transform firePoint;
    public static bool facingRight;
    public Transform BulletTrailPrefab;
    bool flipped = true;
    public static Vector3 hitPos;
    public static Vector3 hitNormal;
    public Transform HitPrefab;
    public float camShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;
    Player player = null;
    public Animator animator;
    public Transform meleePoint;
    public float meleeRange = 0.5f;
    public LayerMask enemiesLayer;
   
    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firePoint? HUH?!");
        }
    }

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null) Debug.LogError("No CameraShake script found on GM object");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Damage = player.stats.curAttack;
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.stats.curClass) {
            //add more cases for each additional class
        case Player.Classes.Wizard: 
            if (fireRate == 0) {
                if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
                    Shoot(UnityStandardAssets._2D.PlatformerCharacter2D.m_FacingRight);
                }
            }    
            else {
                if (CrossPlatformInputManager.GetButton("Fire1") && Time.time > timeToFire) {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot(UnityStandardAssets._2D.PlatformerCharacter2D.m_FacingRight); 
                }
            } 
            break;
        case Player.Classes.Knight: 
            if (CrossPlatformInputManager.GetButtonDown("Fire1")){ MeleeAttack(); }
            break;
        }
        
    }

    void Shoot (bool facing)
    {
        // Debug.Log("Test");
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 right = new Vector2(1, 0);
        Vector2 left = new Vector2(-1, 0);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, right, 100, toHit);
       
        //Effect();
        if (facing)
        {
            hit = Physics2D.Raycast(firePointPosition, right, 100, toHit);
            Debug.DrawLine(firePointPosition, right * 100);
        }
        else
        {
            hit = Physics2D.Raycast(firePointPosition, left, 100, toHit);
            Debug.DrawLine(firePointPosition, left * 100);
        }
        if (hit.collider != null)
        {
            
            hitPos = hit.point;
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            //Debug.Log("we hit " + hit.collider.name + " and did " + Damage + " damage!" + hit.point);
            
        }
        
        if (Time.time >= timeToSpawnEffect)
        {
            if ((hit.collider == null) && (facing))
            {
                hitPos = (firePointPosition + (30 * right));
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else if ((hit.collider == null) && (!facing))
            {
                hitPos = (firePointPosition + (30 * left));
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            if(player.stats.curStaminaLeft >= 2){
                Effect(facing, hitPos, hitNormal);
                timeToSpawnEffect = Time.time + 1 / effectSpawnRate;  
            }
        }
    }

    void MeleeAttack() {
        if (player.stats.curStaminaLeft >= 1){
            // Play an attack animation
            animator.SetTrigger("Attack");
            // Detect enemies in range of attack 
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemiesLayer);
            // Damage enemies
            foreach(Collider2D enemy in hitEnemies) {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().DamageEnemy(2 * player.stats.curAttack);
            }
            player.stats.curStaminaLeft -= 1;
            //Debug.Log("Swinging the Sword!!!");
        }
    }

    void OnDrawGizmosSelected() {
        if (meleePoint == null) return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    void Effect(bool facing, Vector3 hitPos, Vector3 hitNormal)
    {
        if (!facing)
        {
            if (flipped)
            {
                Vector3 theScale = BulletTrailPrefab.localScale;
                theScale.x *= -1;
                BulletTrailPrefab.localScale = theScale;
                flipped = false;
            }
            Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            if (!flipped)
            {
                flipped = true;
                Vector3 theScale = BulletTrailPrefab.localScale;
                theScale.x *= -1;
                BulletTrailPrefab.localScale = theScale;
            }
            Transform ball = BulletTrailPrefab;
            
            Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        }
        player.stats.curStaminaLeft -= 2;
        //shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);
    }
}
