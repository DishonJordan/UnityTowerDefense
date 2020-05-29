using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{    
    [Serializable]
    public struct ListWrapper
    {
        //wave[0] is enemy index
        //wave[1] is time gap between this enemy and the next
        public List <Vector2> wave;
        public float waveDelay;
    }

    //[Header("Spawner properties")]
    public GameObject[] enemies;
    public List <ListWrapper> waves;
    public bool waveEnded;
    private float timer;
    private float currentDelay;
    //[Header("Enemy dependencies")]
    public Waypoints waypoints;
    public Bank bank;
    public GameManager gm;

    public int waveIndex;
    public int wavesSurvived;
    public static int enemiesAlive;
    private int enemyIndex;
    public float delayBetweenWaves;
    public float waveTimer; // timer to countdown secs until next wave
    public bool spawningWave; // used to disable nextwave caller while spawning 

    private void Awake()
    {
        if (waypoints == null)
            Debug.LogError("Spawner needs Waypoints!", this);
        if (bank == null)
            Debug.LogError("Spawner needs Bank!", this);
        if (gm == null)
            Debug.LogError("Spawner needs GameManager!", this);
    }

    private void Start(){
        wavesSurvived = 0;
        enemiesAlive = 0;
        waveEnded = true;
        delayBetweenWaves = 0;
        waveTimer = 0;
        spawningWave = false;
    }

    private void Update()
    {
        if(waveIndex == waves.Count && enemiesAlive == 0 && !GameManager.gameIsOver)
        {
            gm.GameWon();
        }
        if(!waveEnded){
            if(timer >= currentDelay){
                if(enemyIndex >= waves[waveIndex].wave.Count){
                    EndWave();
                    return;
                }
                GameObject newEnemy = Instantiate(enemies[(int)waves[waveIndex].wave[enemyIndex][0]], transform.position, transform.rotation);

                // Inject scene dependencies into enemy
                Enemy e = newEnemy.GetComponentInChildren<Enemy>();
                e.waypoints = waypoints;
                e.distanceToEnd = e.waypoints.lengthOfPath;
                e.bank = bank;
                e.gm = gm;

                currentDelay = waves[waveIndex].wave[enemyIndex][1];
                enemyIndex++;
                enemiesAlive++;
                timer = 0;
                spawningWave = true;
            }
        } 
        if  (waveTimer <= delayBetweenWaves && !spawningWave && wavesSurvived > 0) 
        {
            waveTimer += Time.deltaTime;
        }
        timer += Time.deltaTime;
    }

    public void BeginWave(int waveNumber, bool earlyCall){
        waveIndex = waveNumber - 1;
        if(waveIndex < waves.Count){
            waveEnded = false;
            enemyIndex = 0;
            if(earlyCall)
            {
                currentDelay = 0;
                delayBetweenWaves = 0;
                waveTimer = 0;
            } else 
            {
                currentDelay = waves[waveIndex].waveDelay;
                delayBetweenWaves = currentDelay;
            }
            timer = 0;
        }
    }

    private void EndWave(){
        waveEnded = true;
        wavesSurvived++;
        spawningWave = false;
        waveTimer = 0;
        gm.WaveEnded();
    }
}
