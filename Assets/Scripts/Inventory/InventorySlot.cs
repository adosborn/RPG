using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Button useButton; 
    public Text useText;
    Item item;


    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot (){
        //GameMaster.gm.SpawnItemOnPlayer(item);
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        GameMaster.gm.SpawnItemOnPlayer(item);
        //item.hasBeenPickedUp = false;
        Inventory.instance.Remove(item);
        useButton.interactable = false;
        useText.enabled = false; 
    }

    public void SelectItem() 
    {
        if (useButton.interactable == false && item != null) {
            useButton.interactable = true;
            removeButton.interactable = true;
            useText.enabled = true; 
            if (item.isEquipment) {
                Debug.Log("trying to write");
                EquipmentManager.instance.SelectedStats((Equipment)item);
            }
        }
        else {
            useButton.interactable = false;
            removeButton.interactable = false;
            useText.enabled = false; 
        }
    }

    public void UseItem ()
    {
        if (item != null)
        {
            item.Use();
            useButton.interactable = false;
            removeButton.interactable = false;
            useText.enabled = false; 
            EquipmentManager.instance.deselectSlots();
        }
    }
}
