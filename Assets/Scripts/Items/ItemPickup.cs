using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemPickup : Interactable
{
    public Item item;
    public GameObject pickUpButton;
    public bool pickUpEnabled = false;
    public float curDistance = 9999f; 
    bool listenerAdded = false;
    

    private void Awake() {
        pickUpButton = GameObject.FindGameObjectWithTag("PickUp");
        if (pickUpButton != null) {
            pickUpButton.GetComponent<Button>().onClick.AddListener(PickUpClicked);
        }
        //pickUpButton.SetActive(false);
    }

    private void FixedUpdate() {
        if (pickUpButton != null) {
            if (!listenerAdded) {
                pickUpButton.GetComponent<Button>().onClick.AddListener(PickUpClicked);
                listenerAdded = true;
            }
            if (GameMaster.InteractingSize() >= 1) { 
                pickUpButton.GetComponent<Button>().interactable = true;
                //Debug.Log("PickUp button should be showing...");
            }
            else { pickUpButton.GetComponent<Button>().interactable = false;}
        }
        else {
            pickUpButton = GameObject.FindGameObjectWithTag("PickUp");
        }
    }

    public void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);
        GameMaster.removeInteracting(item);
        if (wasPickedUp)
        {
            pickUpEnabled = false;
            this.transform.parent.gameObject.GetComponent<ItemHolderObject>().hasBeenPickedUp = true;
            Destroy(gameObject);
        }
    }

    public override void Interact(float distance)
    {
        if (item.isCoin) {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.ChangeCoins(item.value);
            //Debug.Log("Trying to pick up coin");
            Destroy(this.transform.parent.gameObject);
            //Destroy(gameObject);
        }
        else if(item.isKey){
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.stats.curKeys ++;
            Destroy(this.transform.parent.gameObject);
            Debug.Log(player.stats.curKeys);
        }
        else if(item.isPotion){
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.stats.curHealth += item.healthChange;
            player.StartDamageNum((-item.healthChange), this.transform.position, this.transform.rotation);
            //statusIndicator.SetHealth(player.stats.curHealth, player.stats.maxHealth);
            Destroy(this.transform.parent.gameObject);
        }
        else {
            curDistance = distance;
            GameMaster.addInteracting(item);
        }
        //Debug.Log("trying to add " + item.name);
    }

    public override void NotInteracting(float distance) {
        curDistance = distance;
        GameMaster.removeInteracting(item);
    }

    public void PickUpClicked()
    {
        if (GameMaster.InteractingSize() >= 1 && curDistance <= radius) {
            pickUpEnabled = false;
            pickUpButton.GetComponent<Button>().interactable = false;
            pickUpEnabled = false;
            PickUp();
            //Debug.Log("Picking up (interaction)");
        }
        pickUpButton.GetComponent<Button>().interactable = false;
    }
}
