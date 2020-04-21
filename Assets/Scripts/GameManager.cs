using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int baseHealth;

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
    }

    void GameEnded(){
        Debug.Log("Game has ended");
    }
}
