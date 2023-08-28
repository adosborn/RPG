using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.MonoBehaviour;

public class SkeletonRun : StateMachineBehaviour
{
    public float speed = 2.5f;

    public Transform player;
    Rigidbody2D rb;
    public Vector2 pastPos = new Vector2(0,0);
    Skeleton skeleton;
    public float attackRange = 3.0f;
    //bool waiting = false;
    
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
            skeleton.LookAtPlayer();
            if (player == null) {player = GameObject.FindGameObjectWithTag("Player").transform;}
            Vector2 newPos = new Vector2(-1000f,-1000f);
            if(rb.position.x >= pastPos.x - 0.03 && rb.position.x <= pastPos.x + 0.03 && rb.position.y >= pastPos.y - 0.03 && rb.position.y <= pastPos.y + 0.03) {
                newPos = Vector2.MoveTowards(rb.position, new Vector2(player.position.x, rb.position.y + 10), 3 * speed * Time.fixedDeltaTime);
                animator.GetComponent<Grounded>().active = false;
                skeleton.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 20f), ForceMode2D.Force);
                //Debug.Log("trying to go up");
            }
            else {
                Vector2 target = new Vector2(player.position.x, rb.position.y);
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
