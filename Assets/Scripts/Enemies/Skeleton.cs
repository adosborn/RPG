using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Transform player;

    public bool isFlipped;
    bool waiting = false;

    public void LookAtPlayer() {
        if (player != null) {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x < player.position.x && isFlipped){
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if(transform.position.x > player.position.x && !isFlipped){
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }
    }

    public void LookAwayFromPlayer() {
        if (player != null) {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped){
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if(transform.position.x < player.position.x && !isFlipped){
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }
    }

    void Update() {
        if (player == null)
        {
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
    }

    public void SearchForPlayerFromOutside(){
        StartCoroutine(SearchForPlayer());
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            waiting = false;
            player = sResult.transform;
            GetComponent<Animator>().SetBool("Walking", true);
            yield return false;
        }
    }
}
