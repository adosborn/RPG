using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefault = false;
    public bool isCoin = false;
    public bool isKey = false;
    public bool isEquipment = false;
    public bool isPotion = false;
    public int healthChange = 0;
    public int value = 0;
    public int rarity = 0;
    public string description = "";
    public float sizeMultiplier = 1.0f;

    public virtual void Use()
    {
        //use the item
        //somthing might happen
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory () {
        Inventory.instance.Remove(this);
    }
}
