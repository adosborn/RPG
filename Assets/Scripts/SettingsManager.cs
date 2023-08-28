using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //public GameObject gameMaster;
    public GameObject settingsBox;
    public Text wizardStats;
    public Text knightStats;
    //public Text otherStats;
    Player player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void updateClassText() {
        wizardStats.text = "Lv. " + player.stats.curLvl[(int)Player.Classes.Wizard] + "\n" + 
        player.stats.curXP[(int)Player.Classes.Wizard] + "/" + 
        player.stats.levelXP[(int)Player.Classes.Wizard] + "XP";

        knightStats.text = "Lv. " + player.stats.curLvl[(int)Player.Classes.Knight] + "\n" + 
        player.stats.curXP[(int)Player.Classes.Knight] + "/" + 
        player.stats.levelXP[(int)Player.Classes.Knight] + "XP";
    }

    public void showBox()
    {
        settingsBox.GetComponent<Renderer>().enabled = true;
    }

    public void goToMain()
    { 
        SceneManager.LoadScene(0);
    }

    public void saveCurrentPlayer() 
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.SavePlayer();
    }
    
}
