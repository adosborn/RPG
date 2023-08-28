using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    Transform player;
    private float distance = 100000f;

    public virtual void Interact(float distance)
    {
        // This method is meant to be overwriten
        //Debug.Log("interacting" + distance);
    }
    public virtual void NotInteracting(float distance){
        //Debug.Log("no longer interacting");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Start()
    {
        //Debug.Log("looking for a player");
        GameObject playerGm = GameObject.FindGameObjectWithTag("Player");
        if (playerGm != null) {
            player = playerGm.transform;
        }
        
        //Debug.Log(player.position);
    }
    private void Update()
    {
        if (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                //Debug.Log("looking for a player");
                return;
            }
        }
        if (player != null) {distance = Vector3.Distance(player.position, transform.position); }
        else {distance = 100000f; }
        if (distance <= radius){
            Interact(distance);
        }
        else {
            NotInteracting(distance);
        }
        
    }
    
}
