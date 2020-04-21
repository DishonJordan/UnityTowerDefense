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
        if(Health < 0){ Health = 0;}
    }

    void GameEnded(){
        Debug.Log("Game has ended");
    }
}
