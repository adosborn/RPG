using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour{

    //public Player prefabPlayer;

    public void StartGame()
    {
        //GameMaster.loadPlayer();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("We Quit the Game");
        Application.Quit();
    }

    public void LoadPlayer(Player curPlayer)
    {
        SaveData data = SaveSystem.LoadPlayer();

        SceneManager.LoadScene(data.sceneNum);
        curPlayer.stats.curHealth = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        curPlayer.transform.position = position;
    }

    // public void NewGame(){
    //     // SaveData data = SaveSystem.LoadPlayer();

    //     // data.sceneNum = 1;
    //     // data.health = 100;
    //     // data.position[0] = 0f; data.position[1] = 1.5f; data.position[2] = 0f;
    //     // data.level[0] = 1; data.level[1] = 1;
    //     // data.xp[0] = 0; data.xp[1] = 0;
    //     // data.xpNeeded[0] = 20; data.xpNeeded[1] = 20;
    //     // data.coins = 0; 
    //     // data.classNum = 0;
    //     // data.inventoryItems = null;
    //     // data.equipmentItems = null;
    //     // data.pickedUpIDs = null;

    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //     //SaveSystem.SavePlayer(curPlayer);
    // }
}
