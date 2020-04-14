using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    private List <List <int>> waves;

    [Header("Spawner properties")]
    public GameObject[] enemies; 
    public int timeBetweenWaves;
    public int timeBetweenSpawns;
    private float timer;

    [Header("Enemy dependencies")]
    public Waypoints waypoints;
    public Bank bank;

    private bool spawningWave;
    private int waveIndex;
    private int enemyIndex;

    private void Awake()
    {
        if (waypoints == null)
            Debug.LogError("Spawner needs Waypoints!", this);
        if (bank == null)
            Debug.LogError("Spawner needs Bank!", this);
    }

    private void Start(){
        timer = timeBetweenWaves;
        spawningWave = false;
        waveIndex = 0;
        enemyIndex = 0;
        List<int> wave1 = new List<int>{0,0,0};
        List<int> wave2 = new List<int>{0,0,0,0,0};
        List<int> wave3 = new List<int>{0,0};
        List<int> wave4 = new List<int>{0,0,0,0,0,0,0};
        waves = new List<List<int>>{wave1, wave2, wave3, wave4};
    }

    private void Update()
    {
        if(!spawningWave){
            if(timer >= timeBetweenWaves){
                timer = timeBetweenSpawns;
                spawningWave = true;
            }
        }
        else{
            if(timer >= timeBetweenSpawns){
                if(waveIndex < waves.Count){

                    GameObject newEnemy = Instantiate(enemies[waves[waveIndex][enemyIndex]], transform.position, transform.rotation);

                    // Inject scene dependencies into enemy
                    Enemy e = newEnemy.GetComponent<Enemy>();
                    e.waypoints = waypoints;
                    e.bank = bank;

                    if(++enemyIndex == waves[waveIndex].Count){
                        enemyIndex = 0;
                        waveIndex++;
                        spawningWave = false;
                    }
                }
                timer = 0;
            }
        }
        timer += Time.deltaTime;
    }
}
