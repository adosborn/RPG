using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    public GameObject[] items;
    public GameObject ogPlayer;
    public Player player;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        items = GameObject.FindGameObjectsWithTag("Item");
    }
    void Start() {
        GameObject pickUpButton = GameObject.FindGameObjectWithTag("PickUp");
        //pickUpButton.GetComponent<Button>().interactable = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float enemyDifficultyModifier = 1.0f;
    public int spawnDelay = 2;
    public Transform spawnPrefab;
    public static List<Item> interacting = new List<Item>();
    public GameObject holderPrefab;
    public GameObject fightButton;
    public Sprite wizardImage; public Sprite knightImage; 


    public IEnumerator RespawnPlayer (bool respawn){
        Debug.Log("Add waiting for spawn sound");
        yield return new WaitForSeconds(spawnDelay);
        if (GameObject.FindGameObjectWithTag("Player") == null){
            if (respawn){
                Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                EquipmentManager.instance.refreashStats();
                player.stats.curHealth = player.stats.maxHealth;
                gm.StartCoroutine(gm.InvinciblePlayer(player));
            }
            else{
                SceneManager.LoadScene(1);
            }
           
        } 
    }

    public IEnumerator InvinciblePlayer(Player player) {
        Debug.Log("INVINCIBLE");
        player.invincible = true;
        yield return new WaitForSeconds(2);
        player.invincible = false;
    }

    public void SpawnItemOnPlayer(Item item) {
        //ItemHolderObject newItem = new ItemHolderObject(holderPrefab, item, false,
        //player.transform.position, item.name + Random.Range(0.0f, 10.0f), SceneManager.GetActiveScene().buildIndex);
        SpawnItemOnPosition(item, player.transform.position);
    }
    public void SpawnItemOnPosition(Item item, Vector3 position) {
        ItemHolderObject newItem = holderPrefab.GetComponent<ItemHolderObject>();
        newItem.item = item;
        //newItem.transform.localScale *= item.sizeMultiplier;
        newItem.hasBeenPickedUp = false;
        newItem.position = position;
        newItem.holderName = item.name + Random.Range(0.0f, 10.0f);
        newItem.scene = SceneManager.GetActiveScene().buildIndex;
        newItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.icon;
        newItem.transform.GetChild(0).GetComponent<ItemPickup>().item = item;
        ItemSpawner.instance.addItem(newItem);
    }

    public static void KillPlayer (Player player, Transform spawnPoint){
        bool shouldRespawn = player.hasRespawn;
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer(shouldRespawn));
    }

    public static void KillEnemy (Enemy enemy, Transform objectSpawn){
        Destroy(enemy.gameObject);
    }

    public static void addInteracting(Item item) {
        if (!interacting.Contains(item)){
            interacting.Add(item);
        }
    }
    public static void removeInteracting(Item item) {
        if (interacting.Contains(item)){
            interacting.Remove(item);
        }
    }
    public static int InteractingSize() {
        return interacting.Count;
    }

    public void SwitchToKnight() { 
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.stats.curClass = Player.Classes.Knight; 
        player.statusIndicator.SetXP(player.stats.curLvl[(int)player.stats.curClass],
        player.stats.curXP[(int)player.stats.curClass],
        player.stats.levelXP[(int)player.stats.curClass]);
        fightButton.GetComponent<Image>().sprite = knightImage;
    }
    public void SwitchToWizard() { 
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.stats.curClass = Player.Classes.Wizard; 
        player.statusIndicator.SetXP(player.stats.curLvl[(int)player.stats.curClass],
        player.stats.curXP[(int)player.stats.curClass],
        player.stats.levelXP[(int)player.stats.curClass]);
        fightButton.GetComponent<Image>().sprite = wizardImage;
    }
}
