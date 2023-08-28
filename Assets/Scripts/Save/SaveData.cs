using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    
    public int sceneNum;
    public int health;
    public float[] position;
    public int[] level;
    public int[] xp;
    public int[] xpNeeded;
    public int coins; 
    public int classNum;
    public string[] inventoryItems;
    public string[] equipmentItems;
    public string[] pickedUpIDs;
    public int[] highscoreArray = {0,0,0,0,0};
    public int score; 
    // public int attack; public int defence; public int speed; public int stamina; public int jump;

    
    public SaveData (Player player)
    {
        //Saving inventory
        List<string> itemsInInventory = new List<string>();
        Inventory inventory;
        inventory = Inventory.instance;
        if (inventory.items != null){
            foreach (Item item in inventory.items){
                itemsInInventory.Add(item.name);
                //Debug.Log(item.name);
            }
        }
        inventoryItems = itemsInInventory.ToArray();

        //Saving equipment
        List<string> itemsEquiped = new List<string>();
        EquipmentManager equipmentManager;
        equipmentManager = EquipmentManager.instance;
        for (int i=0; i<equipmentManager.currentEquipment.Length; i++){
            if (equipmentManager.currentEquipment[i] != null){
                //Debug.Log(equipmentManager.currentEquipment[i].name);
                itemsEquiped.Add(equipmentManager.currentEquipment[i].name);
            }
            else { 
                //Debug.Log("No name property on: " + i);
            }
        }
        equipmentItems = itemsEquiped.ToArray();

        //Saving scene
        sceneNum = SceneManager.GetActiveScene().buildIndex;

        //Saving position
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        //Saving stats 
        if (player.stats.curHealth != 0){ health = player.stats.curHealth; }
        else { 
            if (player.stats.maxHealth != 0){ health = player.stats.maxHealth; }
            else { health = 100;}
        }
        if (player.stats.curLvl != null){ level = player.stats.curLvl; }
        else { level = correctArray(1); }
        if (player.stats.curXP != null){ xp = player.stats.curXP; }
        else { xp = correctArray(0); }
        classNum = (int)player.stats.curClass;
        xpNeeded = player.stats.levelXP;
        coins = player.stats.curCoins;
        // attack = player.stats.curAttack;
        // defence = player.stats.curDefence;
        // speed = player.stats.curSpeed;
        // stamina = player.stats.curStamina;
        // jump = player.stats.curJump;

        //Saving items in sceane
        List<string> itemsInScene = new List<string>();
        GameObject[] itemHoldersGMs = GameObject.FindGameObjectsWithTag("ItemHolder");
        ItemHolderObject[] itemHolders = new ItemHolderObject[itemHoldersGMs.Length];
        for (int i = 0; i < itemHoldersGMs.Length; i++) {
            itemHolders[i] = itemHoldersGMs[i].GetComponent<ItemHolderObject>();
        }
        foreach (ItemHolderObject item in itemHolders){
            //Debug.Log(item.hasBeenPickedUp);
            if(item.hasBeenPickedUp){
                itemsInScene.Add(item.holderName);
                //Debug.Log(item.holderName);
            }
        }
        pickedUpIDs = itemsInScene.ToArray();
        
        //Saving Highscores 
        if (player.highscores != null){
            highscoreArray = player.highscores;
        }

        //Saving Score
        score = player.stats.curScore;
        
        //Print results
        //Debug.Log(coins + " coins, " + xp[0] + " xp, " + level[0] + " level, " + health + " health" + position[1] + " y-value");
    }

    int[] correctArray(int num) {
        int howLong = System.Enum.GetNames(typeof(Player.Classes)).Length;
        int[] newArray = new int[howLong];
        for (int i=0; i<newArray.Length; i++){
            newArray[i]=0;
        }
        return newArray;
    }
}
