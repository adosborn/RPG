using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {Spawning, Waiting, Counting, NotSpawning};


    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public string tagOfEnemy;
    private bool waitForAllDead = true;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.Counting;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update(){  
        if(state == SpawnState.Waiting){
            if (!waitForAllDead){
                WaveCompleted();
            }
            else if (!EnemyIsAlive(tagOfEnemy)){
                WaveCompleted();
            }
            else return;
        }

        if (waveCountdown <= 0){
            if (state != SpawnState.Spawning){
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else{
            waveCountdown -= Time.deltaTime;
        }
    }

    public void Restart(bool waitForAllDeadRestart, float newTimeBetweenFaves) {
        //Debug.Log(state);
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;
        waitForAllDead = waitForAllDeadRestart;
        timeBetweenWaves = newTimeBetweenFaves;
        //Debug.Log(state);
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Compleated!");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0; //to loop
            Debug.Log("All Waves Complete!");
            //return; to not loop
        }
        else
        {
            nextWave++;
        }   
    }

    bool EnemyIsAlive(string tag)
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag(tag) == null) return false;
        }
        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy (Transform _enemy)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    public void StopSpawning (){
        state = SpawnState.NotSpawning;
        gameObject.GetComponent<WaveSpawner>().enabled = false;
    }
}
