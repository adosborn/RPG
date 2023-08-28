using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour{
    #region Singleton

    public static ItemDictionary instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ItemDictionary found!");
            return;
        }
        instance = this;
    }
    #endregion

    public Dictionary<string, Item> allItems = new Dictionary<string, Item>();
    public Item basicSword; public Item basicStaff; 
    public Item leatherHat; public Item leatherChestplate; public Item leatherPants; 
    public Item metalHat; public Item metalChestplate; public Item metalPants; 
    public Item fireball; public Item coin; public Item healthPotion;

    void OnEnable() {
        allItems.Add("Basic Sword", basicSword); allItems.Add("Wood Staff", basicStaff); 
        allItems.Add("Leather Helmet", leatherHat); allItems.Add("Leather Chestplate", leatherChestplate); allItems.Add("Leather Pants", leatherPants);
        allItems.Add("Metal Helmet", metalHat); allItems.Add("Metal Chestplate", metalChestplate); allItems.Add("Metal Pants", metalPants);
        allItems.Add("Fireball", fireball); allItems.Add("Coin", coin); allItems.Add("HealthPotion", healthPotion);
    }
}
