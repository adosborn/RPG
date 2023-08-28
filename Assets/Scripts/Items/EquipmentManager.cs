using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region singleton
    public static EquipmentManager instance;
    void Awake() {
        instance = this;
    }
    #endregion
    public Equipment[] currentEquipment;
    public Image helmetSlot; public Image chestplateSlot; public Image legsSlot; public Image weaponSlot; public Image itemSlot;
    public Text helmetText; public Text chestplateText; public Text legsText; public Text weaponText; public Text itemText;
    public Text attackText; public Text defenceText; public Text speedText; public Text staminaText; public Text jumpText;
    public Text attackChangeText; public Text defenceChangeText; public Text speedChangeText; public Text staminaChangeText; public Text jumpChangeText;

    public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    public Player player;

    Inventory inventory;

    void Start() {
        //instance = this;
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        //Debug.Log(numSlots);
        currentEquipment = new Equipment[numSlots];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        onEquipmentChanged += modifyStats;
        onEquipmentChanged += VisualEquip;
    }

    public void Equip (Equipment newItem) {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = null;
        //Debug.Log(slotIndex);
        //Debug.Log(currentEquipment.Length);
        if (currentEquipment[slotIndex] != null) {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if(onEquipmentChanged != null) {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        inventory.Remove(newItem);
        currentEquipment[slotIndex] = newItem;
    }
    public void refreashStats() {
        StartCoroutine(refreashing());
    }
    IEnumerator refreashing() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        yield return new WaitUntil(() => player != null);
        foreach (Equipment equipment in currentEquipment){
            if (equipment != null) {
                player.stats.curAttack += equipment.damageModifier;
                player.stats.curDefence += equipment.armorModifier;
                player.stats.curSpeed += equipment.speedModifier;
                player.stats.curStamina += equipment.staminaModifier;
                player.stats.curJump += equipment.jumpModifier;
            }
        }
        
    }

    public void VisualEquip(Equipment newE, Equipment oldE){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        switch (newE.equipSlot) {
            case EquipmentSlot.Head: {
                player.hair.enabled = false; 
                player.helmet.enabled = true;
                player.helmet.sprite = newE.sprites[0];
                //Debug.Log("EQUIPING TO HEAD" + player.helmet.sprite);
                break;
            }  
            case EquipmentSlot.Chest: {
                player.chestplate.enabled = true;
                player.chestplate.sprite = newE.sprites[0];
                player.arm1.enabled = true;
                player.arm1.sprite = newE.sprites[1];
                player.arm2.enabled = true;
                player.arm2.sprite = newE.sprites[1];
                //Debug.Log("EQUIPING TO CHEST");
                break;
            }   
            case EquipmentSlot.Legs: {
                player.leg1.enabled = true;
                player.leg1.sprite = newE.sprites[0];
                player.leg2.enabled = true;
                player.leg2.sprite = newE.sprites[1];
                // Debug.Log("EQUIPING TO LEGS");
                break;
            }  
            case EquipmentSlot.Weapon: {
                player.weapon.enabled = true;
                player.weapon.sprite = newE.sprites[0];
                //Debug.Log("EQUIPING TO WEAPON");
                break;
            }  
            case EquipmentSlot.Item: {
                Debug.Log("EQUIPING TO ITEM");
                break;
            }   
        }
    }

    public void Unequip (int slotIndex) {
        if (currentEquipment[slotIndex] != null) {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;
            if(onEquipmentChanged != null) {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void SelectedStats(Equipment item) {
        //Debug.Log("trying harder to write");
        if (currentEquipment[(int)item.equipSlot] != null) {
            //attack
            if (item.damageModifier - currentEquipment[(int)item.equipSlot].damageModifier > 0) {
                attackChangeText.text = ("+"+(item.damageModifier - currentEquipment[(int)item.equipSlot].damageModifier));
                attackChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                attackChangeText.text = (item.damageModifier - currentEquipment[(int)item.equipSlot].damageModifier) + "";
                attackChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            //defence
            if (item.armorModifier - currentEquipment[(int)item.equipSlot].armorModifier > 0) {
                defenceChangeText.text = ("+"+(item.armorModifier - currentEquipment[(int)item.equipSlot].armorModifier));
                defenceChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                defenceChangeText.text = (item.armorModifier - currentEquipment[(int)item.equipSlot].armorModifier) + "";
                defenceChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            //speed
            if (item.speedModifier - currentEquipment[(int)item.equipSlot].speedModifier > 0) {
                speedChangeText.text = ("+"+(item.speedModifier - currentEquipment[(int)item.equipSlot].speedModifier));
                speedChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                speedChangeText.text = (item.speedModifier - currentEquipment[(int)item.equipSlot].speedModifier) + "";
                speedChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            //stamina
            if (item.staminaModifier - currentEquipment[(int)item.equipSlot].staminaModifier > 0) {
                staminaChangeText.text = ("+"+(item.staminaModifier - currentEquipment[(int)item.equipSlot].staminaModifier));
                staminaChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                staminaChangeText.text = (item.staminaModifier - currentEquipment[(int)item.equipSlot].staminaModifier) + "";
                staminaChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            //jump
            if (item.jumpModifier - currentEquipment[(int)item.equipSlot].jumpModifier > 0) {
                jumpChangeText.text = ("+"+(item.jumpModifier - currentEquipment[(int)item.equipSlot].jumpModifier));
                jumpChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                jumpChangeText.text = (item.jumpModifier - currentEquipment[(int)item.equipSlot].jumpModifier) + "";
                jumpChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            //add more as applicible 
        }
        else {
            if (item.damageModifier > 0) {
                attackChangeText.text = "+"+item.damageModifier; 
                attackChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                attackChangeText.text = item.damageModifier+"";
                attackChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            if (item.armorModifier > 0) {
                defenceChangeText.text = "+"+item.armorModifier;
                defenceChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                defenceChangeText.text = item.armorModifier+"";
                defenceChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            if (item.speedModifier > 0) {
                speedChangeText.text = "+"+item.speedModifier;
                speedChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                speedChangeText.text = item.speedModifier+"";
                speedChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            if (item.staminaModifier > 0) {
                staminaChangeText.text = "+"+item.staminaModifier;
                staminaChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                staminaChangeText.text = item.staminaModifier+"";
                staminaChangeText.color = new Color(0.8f,0f,0f,1f);
            }
            if (item.jumpModifier > 0) {
                jumpChangeText.text = "+"+item.jumpModifier;
                jumpChangeText.color = new Color(0f,0.8f,0f,1f);
            }
            else {
                jumpChangeText.text = item.jumpModifier+"";
                jumpChangeText.color = new Color(0.8f,0f,0f,1f);
            }
        }
    }
    public void deselectSlots() {
        attackChangeText.text = "";
        defenceChangeText.text = "";
        speedChangeText.text = "";
        staminaChangeText.text = "";
        jumpChangeText.text = "";
    }

    public void UnequipAll() {
        for (int i = 0; i < currentEquipment.Length; i++) {
            Unequip(i);
        }
    }
    void modifyStats(Equipment oldE, Equipment newE) {
        if (oldE != null && newE != null) {
            player.stats.curAttack -= (newE.damageModifier - oldE.damageModifier);
            player.stats.curDefence -= (newE.armorModifier - oldE.armorModifier);
            player.stats.curSpeed -= (newE.speedModifier - oldE.speedModifier);
            player.stats.curStamina -= (newE.staminaModifier - oldE.staminaModifier);
            player.stats.curJump -= (newE.jumpModifier - oldE.jumpModifier);
        }
        else {
            if (oldE != null) {
                player.stats.curAttack += oldE.damageModifier;
                player.stats.curDefence += oldE.armorModifier;
                player.stats.curSpeed += oldE.speedModifier;
                player.stats.curStamina += oldE.staminaModifier;
                player.stats.curJump += oldE.jumpModifier;
            }
            else {
                player.stats.curAttack -= newE.damageModifier;
                player.stats.curDefence -= newE.armorModifier;
                player.stats.curSpeed -= newE.speedModifier;
                player.stats.curStamina -= newE.staminaModifier;
                player.stats.curJump -= newE.jumpModifier;
            }
        }
    }
    void Update () {
        //if (input.GetKeyDown(KeyCode.U)) {UnequipAll;}
        if (currentEquipment[0] != null) { 
            helmetSlot.enabled = true;
            helmetSlot.sprite = currentEquipment[0].icon; 
            helmetText.text = currentEquipment[0].description.Replace("\\n", "\n");
            //modifyStats(currentEquipment[0])
        }
        else { helmetSlot.sprite = null; }
        if (currentEquipment[1] != null) { 
            chestplateSlot.enabled = true;
            chestplateSlot.sprite = currentEquipment[1].icon;
            chestplateText.text = currentEquipment[1].description.Replace("\\n", "\n"); 
        }
        else { chestplateSlot.sprite = null; }
        if (currentEquipment[2] != null) { 
            legsSlot.enabled = true;
            legsSlot.sprite = currentEquipment[2].icon; 
            legsText.text = currentEquipment[2].description.Replace("\\n", "\n");
        }
        else { legsSlot.sprite = null; }
        if (currentEquipment[3] != null) { 
            weaponSlot.enabled = true;
            weaponSlot.sprite = currentEquipment[3].icon; 
            weaponText.text = currentEquipment[3].description.Replace("\\n", "\n");
        }
        else { weaponSlot.sprite = null; }
        if (currentEquipment[4] != null) { 
            itemSlot.enabled = true;
            itemSlot.sprite = currentEquipment[4].icon;
            itemText.text = currentEquipment[4].description.Replace("\\n", "\n");
        }
        else { itemSlot.sprite = null; }
        if (attackText != null) {attackText.text = player.stats.curAttack + "";}
        if (defenceText != null) {defenceText.text = player.stats.curDefence + "";}
        if (speedText != null) {speedText.text = player.stats.curSpeed + "";}
        if (staminaText != null) {staminaText.text = player.stats.curStamina + "";}
        if (jumpText != null) {jumpText.text = player.stats.curJump + "";}
    }
}