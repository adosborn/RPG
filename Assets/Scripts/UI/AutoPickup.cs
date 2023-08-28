using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutoPickup : Interactable
{
    public Item item;
    public override void Interact(float distance)
    {
        if (item.isCoin) {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.ChangeCoins(item.value);
            Debug.Log("Trying to pick up coin");
            Destroy(gameObject);
        }
        if (item.isKey) {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.stats.curKeys ++;
            Destroy(gameObject);
            Debug.Log(player.stats.curKeys);
        }
        // This method is meant to be overwriten
        //Debug.Log("interacting" + distance);
    }
}
