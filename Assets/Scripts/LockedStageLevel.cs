using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedStageLevel : MonoBehaviour
{
    //public GameMaster gm;
    public GameObject gameManager;
    public GameObject key;
    public GameObject wall;
    public Transform[] enemySpawnPoints;
    public GameObject[] keySpawnPoints;
    public Player player;
    bool searchingForPlayer = false;
    bool isChecking = false;
    bool foundWall = false;
    public int keysRequired = 5;
    int curentKeys = 0;
    //bool stagePassed = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GM");
        if (gameManager != null){
            Debug.Log("Spawn Points Set");
            gameManager.GetComponent<WaveSpawner>().enabled = true;
            gameManager.GetComponent<WaveSpawner>().spawnPoints = enemySpawnPoints;
            gameManager.GetComponent<WaveSpawner>().Restart(false, 15);
            if (player == null){
                if (!searchingForPlayer){
                    searchingForPlayer = true;
                    StartCoroutine(FindPlayer());
                }
                return;
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if (!isChecking){
            isChecking = true;
            StartCoroutine(CheckDistance());
        }
        if (!foundWall){
            StartCoroutine(LookForWall());
        }
        if (player.stats.curKeys == keysRequired){
            player.stats.curKeys = 0;
            //Destroy(wall.gameObject);
            gameManager.GetComponent<SurfaceGeneration>().DestroyWall();
            //stagePassed = true;
            gameManager.GetComponent<WaveSpawner>().StopSpawning();
            gameObject.GetComponent<WaveSpawner>().StopSpawning();
            gameObject.GetComponent<LockedStageLevel>().enabled = false;
        }
        if (curentKeys != player.stats.curKeys){
            curentKeys = player.stats.curKeys;
            gameManager.GetComponent<SurfaceGeneration>().SetWallCounter((float)curentKeys, (float)keysRequired);
        }
    }

    public IEnumerator CheckDistance() {
        if (gameManager.GetComponent<WaveSpawner>().spawnPoints == this.enemySpawnPoints && gameManager.GetComponent<GameMaster>().player != null){
            if (Mathf.Abs(gameManager.GetComponent<GameMaster>().player.transform.position.x - transform.position.x) >= 100f){
                gameManager.GetComponent<WaveSpawner>().Restart(true, 15);
            }
        }
        yield return new WaitForSeconds(5f);
        isChecking = false;
    }

    public IEnumerator LookForWall() {
        if (wall == null){
            wall = gameManager.GetComponent<SurfaceGeneration>().mostRecentWall;
        }
        else {
            foundWall = true;
            yield break;
        }
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator FindPlayer() {
        Player sResult = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (sResult == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FindPlayer());
        }
        else{
            player = sResult;
            keysRequired = 3 + (int)(player.gameObject.transform.position.x / 100f);
            yield return false;
        }
    }
}
