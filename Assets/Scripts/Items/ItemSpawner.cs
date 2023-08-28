using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    #region Singleton

    public static ItemSpawner instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ItemSpawner found!");
            return;
        }
        instance = this;
    }
    #endregion
    public ItemHolderObject[] items;
    ItemDictionary itemDictionary;

    public void addItem(ItemHolderObject holder) {
        ItemHolderObject[] temp = new ItemHolderObject[items.Length + 1];
        items.CopyTo(temp, 0);
        items = temp;
        items[items.Length - 1] = holder;
        holder.transform.localScale = new Vector3(1,1,1);
        holder.transform.localScale *= holder.item.sizeMultiplier;
        Instantiate(holder, holder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    void Start() {
        itemDictionary = ItemDictionary.instance;
        foreach (ItemHolderObject item in items) {
            if (!item.hasBeenPickedUp) {
                Instantiate(item, item.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
    }

    public void SpawnRandom(float rarity, Vector3 position) {
        int itemRarity = -1;
        List<Item> correctRarityItems = new List<Item>();
        while (itemRarity < 0){
            itemRarity = (int)RandomGaussian(rarity - 2, rarity + 2);
        }
        Debug.Log(itemRarity);
        foreach (Item itemEntry in itemDictionary.allItems.Values){
            if (itemEntry.rarity == itemRarity) {
                correctRarityItems.Add(itemEntry);
            }
        }
        if (correctRarityItems.Count != 0){
            if (position != null) {
                GetComponent<GameMaster>().SpawnItemOnPosition(correctRarityItems[Random.Range(0,correctRarityItems.Count)], position);
            }
            else {
                GetComponent<GameMaster>().SpawnItemOnPlayer(correctRarityItems[Random.Range(0,correctRarityItems.Count)]);
            }
        }
    }

    public static float RandomGaussian(float minValue, float maxValue){
        float u, v, S;
        do{
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);
        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
 
        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }

    public void randomTest(float min, float max) {
        int one = 0;
        int two = 0;
        int three = 0;
        int four = 0;
        int five = 0;
        for (int i = 0; i < 10000; i++) {
            switch((int)RandomGaussian(min,max)){
                case(1):
                    one++; break;
                case(2):
                    two++; break;
                case(3):
                    three++; break;
                case(4):
                    four++; break;
                case(0):
                    five++; break;
            }
        }
        Debug.Log("1:" + one + " 2:" + two + " 3:" + three + " 4:" + four + " 0:" + five);
    }
}
