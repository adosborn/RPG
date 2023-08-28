using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;   
    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;
    public bool active = true;
    private bool faceing;
    private bool waiting = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active){
            m_Grounded = false;
            GetComponent<Skeleton>().LookAtPlayer();
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++){
                if (colliders[i].gameObject != gameObject){
                    m_Grounded = true;
                    if (colliders[i].gameObject.tag == "Edge" && !waiting){
                        if (!GetComponent<Animator>().GetBool("Walking")) {
                            
                            GetComponent<Animator>().SetBool("Walking", true);
                            StartCoroutine(WaitTime());
                            break;
                        }
                        else{
                            GetComponent<Animator>().SetBool("Walking", false);
                            GetComponent<Animator>().SetTrigger("WalkAway");
                            faceing = GetComponent<Skeleton>().isFlipped;
                        }
                    }
                }
            }
            if (!m_Grounded) {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
            }
        }
    }

    IEnumerator WaitTime() {
        waiting = true;
        yield return new WaitForSeconds(2f);
        waiting = false;
    }
}
