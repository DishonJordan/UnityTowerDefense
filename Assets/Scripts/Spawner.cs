﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    [Serializable]
    public struct ListWrapper
    {
        public List <int> wave;
    }

    public List <ListWrapper> waves;

    //[Header("Spawner properties")]
    public GameObject[] enemies; 
    public int timeBetweenWaves;
    public int timeBetweenSpawns;
    private float timer;

    //[Header("Enemy dependencies")]
    public Waypoints waypoints;
    public Bank bank;
    public GameManager gm;
    private bool spawningWave;
    public int waveIndex;
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
        timer = timeBetweenWaves;
        spawningWave = false;
        waveIndex = 0;
        enemyIndex = 0;
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
                    GameObject newEnemy = Instantiate(enemies[waves[waveIndex].wave[enemyIndex]], transform.position, transform.rotation);

                    // Inject scene dependencies into enemy
                    Enemy e = newEnemy.GetComponent<Enemy>();
                    e.waypoints = waypoints;
                    e.bank = bank;
                    e.gm = gm;

                    if(++enemyIndex == waves[waveIndex].wave.Count){
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
