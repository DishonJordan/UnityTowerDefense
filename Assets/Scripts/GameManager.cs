﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;
    public GameObject winUI;
    [SerializeField]
    int baseHealth;
    [SerializeField]
    int wave;
    private Object[] spawners;

    void Start ()
    {
        gameIsOver = false;
        wave = 1;
        spawners = GameObject.FindObjectsOfType(typeof(Spawner));
        BeginNextWave();
    }

    public int Health
	{
		get => baseHealth;
		private set
		{
			baseHealth = value;
			if(baseHealth <= 0){
                GameEnded();
            }
		}
	}

    public void DecrementBy(int amount){
        Health -= amount;
        if(Health < 0){ Health = 0;}
    }

    private void BeginNextWave(){
        foreach(Object x in spawners){
            Spawner y = (Spawner)x;
            y.BeginWave(wave);
        }
    }

    public void WaveEnded(){
        foreach(Object x in spawners){
            Spawner y = (Spawner)x;
            if(!y.waveEnded){
                return;
            }
        }
        wave++;
        BeginNextWave();
    }

    public void GameWon(){
        Debug.Log("Game has been won");
        AudioManager.instance.PlayLevelCompleteMusic();
        gameIsOver = true;
        winUI.SetActive(true);
    }

    void GameEnded(){
        Debug.Log("Game has ended");
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
