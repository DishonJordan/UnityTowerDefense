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
    public static int wavesSurvived;
    public static int enemiesAlive;
    private int enemyIndex;

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
            }
        }
        timer += Time.deltaTime;
    }

    public void BeginWave(int waveNumber){
        waveIndex = waveNumber - 1;
        if(waveIndex < waves.Count){
            waveEnded = false;
            enemyIndex = 0;
            currentDelay = waves[waveIndex].waveDelay;
            timer = 0;
        }
    }

    private void EndWave(){
        waveEnded = true;
        gm.WaveEnded();
    }
}
