using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
	#region Singleton
	public static Bank instance { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // If we change scenes, the bank will stay around.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Destroying Bank because one exists already.");
            Destroy(this);
        }
    }

	#endregion

	public int money;
}
