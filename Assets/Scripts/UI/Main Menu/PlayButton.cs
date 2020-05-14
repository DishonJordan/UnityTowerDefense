using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public int firstLevelIndex;
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelIndex);
    }
}
