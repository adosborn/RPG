using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class Player : MonoBehaviour
{
    public enum Classes { Wizard, Knight }
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth;
        public int[] levelXP;

        private int _curHealth;
        public int curHealth { get { return _curHealth; } set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }}

        private int[] _curXP;
        public int[] curXP { get { return _curXP; } set { _curXP = value; }}

        private int[] _curLvl;
        public int[] curLvl{ get { return _curLvl; } set { _curLvl = value; }}

        private int _curCoins;
        public int curCoins{ get { return _curCoins; } set {_curCoins = Mathf.Clamp(value, 0, 9999); }}

        private int _curKeys;
        public int curKeys{ get { return _curKeys; } set {_curKeys = Mathf.Clamp(value, 0, 9999); }}

        private int _curScore;
        public int curScore{ get { return _curScore; } set {_curScore = Mathf.Clamp(value, 0, 99999); }}

        private int _curAttack;
        public int curAttack{ get { return _curAttack; } set {_curAttack = Mathf.Clamp(value, 0, 99); }}

        private int _curDefence;
        public int curDefence{ get { return _curDefence; } set {_curDefence = Mathf.Clamp(value, 0, 99); }}

        private int _curSpeed;
        public int curSpeed{ get { return _curSpeed; } set {_curSpeed = Mathf.Clamp(value, 0, 99); }}

        private int _curStamina;
        public int curStamina{ get { return _curStamina; } set {_curStamina = Mathf.Clamp(value, 0, 99); }}

        private float _curStaminaLeft;
        public float curStaminaLeft{ get { return _curStaminaLeft; } set {_curStaminaLeft = Mathf.Clamp(value, 0.0f, 99.0f); }}

        private int _curJump;
        public int curJump{ get { return _curJump; } set {_curJump = Mathf.Clamp(value, 0, 99); }}

        
        private Classes _curClass;
        public Classes curClass{ get { return _curClass; } set {_curClass = value; }}

        public void Init()
        {
            levelXP = new int[] {20, 20};
            curHealth = maxHealth;
            curLvl = new int[] {1,1};
            curXP = new int[] {0,0};
            curCoins = 0;
            curKeys = 0;
            curScore = 0;
            curAttack = 10;
            curDefence = 10;
            curSpeed = 10;
            curStamina = 10;
            curStaminaLeft = (float)curStamina;
            curJump = 10;
            curClass = Classes.Wizard;
            //Debug.Log(levelXP.Length);
        }

    }
    [SerializeField]
    Inventory inventory; 
    Highscores highscoreboard;
    ItemDictionary itemDictionary;
    EquipmentManager equipmentManager;
    ItemSpawner itemSpawner;
    public StatusIndicator statusIndicator;
    public bool pickUpButtonClicked;
    public Item mostRecentItem;
    private int numInteractions; 
    bool UpdateingScore = false;
    public GameObject EffectText;
    //public GameObject pickUpButton;
    public GameMaster gm;
    public bool invincible;
    public bool hasRespawn = false;
    public Sprite wizardAttack; public Sprite knightAttack;
    public SpriteRenderer helmet; public SpriteRenderer chestplate; public SpriteRenderer arm1; public SpriteRenderer arm2;
    public SpriteRenderer leg1; public SpriteRenderer leg2; public SpriteRenderer weapon; public SpriteRenderer hair; 
    public int[] highscores = new int[] {0,0,0,0,0};

    void Start()
    { 
        inventory = Inventory.instance;
        highscoreboard = Highscores.instance;
        itemDictionary = ItemDictionary.instance;
        equipmentManager = EquipmentManager.instance;
        itemSpawner = ItemSpawner.instance;
        String highscoreList = "";
        foreach(int highscore in highscores){
            highscoreList += (" " + highscore);
        }
        Debug.Log("Highscores: " + highscoreList);
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            LoadPlayer();
            this.transform.localScale = new Vector3(1.771533f, 1.77153f, 1);
        }
        UnityStandardAssets._2D.PlatformerCharacter2D.m_FacingRight = true;
        if (statusIndicator == null)
        {
            Debug.LogError("No Status Indicator Referenced on PLayer");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    public PlayerStats stats = new PlayerStats();
    public int fallBoundary = -20;

    void Update(){
        if (transform.position.y <= -20){
            this.transform.position = gm.spawnPoint.position;
            DamagePlayer(9999999, transform.position);
        }
        // if (SceneManager.GetActiveScene().buildIndex == 1 && transform.position.x >= 110 && transform.position.y >= 3 && transform.position.y <= 10){
        //     SceneManager.LoadScene(2);
        // }
        if (transform.position.z < -1 || transform.position.z > 1) {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        UpdateStamina();
        if (!UpdateingScore) {
            UpdateingScore = true;
            UpdateScore();
        }
        
    }

    public void UpdateStamina(){
        if (this.stats.curStaminaLeft < this.stats.curStamina) {
            StartCoroutine(IncreaseStamina());
        }
        else{this.stats.curStaminaLeft = this.stats.curStamina;}

        statusIndicator.SetStamina(this.stats.curStaminaLeft, this.stats.curStamina);
    }

    public void UpdateScore(){
        StartCoroutine(DecreaseScore());
        statusIndicator.SetScore(this.stats.curScore);
    }

    public IEnumerator IncreaseStamina() {
        yield return new WaitForSeconds(0.2f);
        this.stats.curStaminaLeft += 0.025f;
    }

    public IEnumerator DecreaseScore() {
        yield return new WaitForSeconds(2f);
        this.stats.curScore -= 1;
        UpdateingScore = false;
    }

    public IEnumerator DamageNum(int dmg, Vector3 dmgLoc, Quaternion rot){
        GameObject dmgNum = Instantiate(EffectText, dmgLoc, transform.rotation);
        if (dmg < 0){
            dmgNum.transform.GetChild(0).GetComponent<Text>().text = ("+"+(dmg*-1));
        }
        else {
            dmgNum.transform.GetChild(0).GetComponent<Text>().text = (dmg + "");
        }
        yield return new WaitForSeconds(1f);
        Debug.Log(dmg);
        Destroy(dmgNum);
    }

    public void StartDamageNum(int dmg, Vector3 dmgLoc, Quaternion rot){
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        StartCoroutine(DamageNum(dmg, dmgLoc, rot));
    }

    public void DamagePlayer(int dmg, Vector3 dmgLoc){
        if (invincible) {return;}
        stats.curHealth -= dmg;
        StartCoroutine(DamageNum(dmg, dmgLoc, transform.rotation));
        if (stats.curHealth <= 0){
            this.transform.position = gm.spawnPoint.position;
            Debug.Log(highscores[0] + "" + highscores[1] + "" + highscores[2] + "" + highscores[3] + "" + highscores[4]);
            if (highscores[highscores.Length-1] != 0){
                if (this.stats.curScore > highscores.Min()){updateHighscores();}
            }
            else{updateHighscores();}
            stats.curScore = 0;
            SavePlayer();
            inventory.items.Clear();
            GameMaster.KillPlayer(this, gm.spawnPoint);
        }
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    public void updateHighscores(){
        highscores[0] = this.stats.curScore;
        Array.Sort(highscores);
        Debug.Log(highscores);
    }

    public void GainXP(int xp){
        //Debug.Log(stats.curXP);
        stats.curXP[(int)stats.curClass] += xp;
        if (stats.curXP[(int)stats.curClass] >= stats.levelXP[(int)stats.curClass]) {GainLvl();}
        statusIndicator.SetXP(stats.curLvl[(int)stats.curClass], stats.curXP[(int)stats.curClass], stats.levelXP[(int)stats.curClass]);
    }

    public void GainLvl() {
        stats.curLvl[(int)stats.curClass] += 1;
        stats.curXP[(int)stats.curClass] = 0;
        stats.levelXP[(int)stats.curClass] += 50;
        statusIndicator.SetXP(stats.curLvl[(int)stats.curClass], stats.curXP[(int)stats.curClass], stats.levelXP[(int)stats.curClass]);
    }

    public void ChangeCoins(int coins) {
        stats.curCoins += coins;
        statusIndicator.SetCoins(stats.curCoins);
    }

    public void SavePlayer(){
        SaveSystem.SavePlayer(this);
    }

    public void ResetPlayer(){
        Debug.Log("Reseting1");
        SaveData data = SaveSystem.LoadPlayer();
        Debug.Log("Reseting2");
        foreach(ItemHolderObject item in gm.GetComponent<ItemSpawner>().items){
            item.hasBeenPickedUp = false;
        }
        SceneManager.LoadScene(1);
        stats.Init();
        equipmentManager.UnequipAll();
        this.transform.position = new Vector3(gm.spawnPoint.position.x, gm.spawnPoint.position.y, 0);
        StartCoroutine(Reseting());
        SaveSystem.SavePlayer(this);
    }

    public IEnumerator Reseting(){
        
        yield return new WaitForSeconds(1);
        if (inventory.items != null){
            inventory.items.Clear();
        }
        stats.Init();
    }

    public void LoadPlayer(){
        SaveData data = SaveSystem.LoadPlayer();

        if (SceneManager.GetActiveScene().buildIndex != data.sceneNum && data.sceneNum != 0){
            //Debug.Log(data.sceneNum + " is the scene");
            SceneManager.LoadScene(data.sceneNum);
        }

        stats.Init();
        this.stats.curHealth = data.health;
        this.stats.curCoins = data.coins;
        this.stats.curLvl = data.level;
        this.stats.levelXP = data.xpNeeded;
        this.stats.curXP = data.xp;
        this.stats.curScore = data.score;
        //Debug.Log(this.stats.curXP);
        this.stats.curClass = (Classes)data.classNum;
        if (this.stats.curClass == Classes.Wizard) {
            GameObject.FindGameObjectWithTag("AttackButton").GetComponent<Image>().sprite = wizardAttack;
        }
        if (this.stats.curClass == Classes.Knight) {
            GameObject.FindGameObjectWithTag("AttackButton").GetComponent<Image>().sprite = knightAttack;
        }
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        foreach (string itemName in data.inventoryItems){
            inventory.Add((Item)itemDictionary.allItems[itemName]);
        }
        foreach (string equipmentName in data.equipmentItems){
            equipmentManager.Equip((Equipment)itemDictionary.allItems[equipmentName]);
        }
        GameObject.FindGameObjectWithTag("SpawnPoint").transform.position = position;
        this.transform.position = position;
        if (data.highscoreArray != null && data.highscoreArray.Length == 5){
            highscores = data.highscoreArray;
            Debug.Log("Highscores uploaded from data");
            highscoreboard.UpdateScores(highscores[4], highscores[3], highscores[2], highscores[1], highscores[0]);
        }
        else {
            highscoreboard.UpdateScores(0,0,0,0,0);
            highscores = new int[] {0,0,0,0,0};
        }
        String highscoreList = "";
        foreach(int highscore in highscores){
            highscoreList += (" " + highscore);
        }
        Debug.Log("Highscores: " + highscoreList);

        
        
        statusIndicator.SetCoins(this.stats.curCoins);
        statusIndicator.SetXP(this.stats.curLvl[(int)stats.curClass], this.stats.curXP[(int)stats.curClass], this.stats.levelXP[(int)stats.curClass]);
        statusIndicator.SetStamina(this.stats.curStamina, this.stats.curStamina);
        //load items in scene
        if (data.pickedUpIDs != null) {
            foreach (string holderId in data.pickedUpIDs){
                foreach (ItemHolderObject holder in itemSpawner.items){
                    if (holder.holderName == holderId) {holder.hasBeenPickedUp = true;}
                }
            }
        }
    }
}
