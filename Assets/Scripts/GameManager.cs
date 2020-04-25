using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;
    public GameObject winUI;
    [SerializeField]
    int baseHealth;
    void Start ()
    {
        gameIsOver = false;
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
    public void GameWon(){
        gameIsOver = true;
        winUI.SetActive(true);
    }
    void GameEnded(){
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
