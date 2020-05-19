using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Win : MonoBehaviour
{
    public Text wave;

    void OnEnable()
    {
       StartCoroutine(AnimateText());
    }
    IEnumerator AnimateText()
    {
        wave.text = "0";
        int round = 0;
        yield return new WaitForSeconds(.7f);
        while(round < Spawner.wavesSurvived)
        {
            round++;
            wave.text = round.ToString();
            yield return new WaitForSeconds(.05f);
        }
    }
    public void Continue()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene("MainMenu");
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

