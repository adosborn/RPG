using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRunAway : StateMachineBehaviour
{
    public float speed = 2.5f;

    public Transform player;
    Rigidbody2D rb;
    public Vector2 pastPos = new Vector2(0,0);
    Skeleton skeleton;
    public float attackRange = 3.0f;
    
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Skeleton>().player != null) {
            player = animator.GetComponent<Skeleton>().player;
        }
        rb = animator.GetComponent<Rigidbody2D>();
        skeleton = animator.GetComponent<Skeleton>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (player != null) {
            skeleton.LookAwayFromPlayer();
            Vector2 newPos = new Vector2(-1000f,-1000f);
            if(rb.position == pastPos) {
                newPos = Vector2.MoveTowards(rb.position, new Vector2(rb.position.x, rb.position.y + 10), 3 * speed * Time.fixedDeltaTime);
                animator.GetComponent<Grounded>().active = false;
            }
            else {
                Vector2 target = new Vector2(rb.position.x + (3 * (rb.position.x-player.position.x)), rb.position.y);
                animator.GetComponent<Grounded>().active = true;
                newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            }
            pastPos = rb.position;
            rb.MovePosition(newPos);

            if (Vector2.Distance(player.position, rb.position)<= attackRange) {
                animator.SetTrigger("Attack");
            }
        }
        else {
            if (animator.GetComponent<Skeleton>().player != null) {
                player = animator.GetComponent<Skeleton>().player;
            }
            else {
                animator.GetComponent<Skeleton>().SearchForPlayerFromOutside();
            }
            animator.SetBool("Walking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
    }
}
