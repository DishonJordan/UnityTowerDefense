using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Win : MonoBehaviour
{
    public Text wave;
    private Spawner spawner;
    private Object[] spawners;
    void OnEnable()
    {
        spawners = GameObject.FindObjectsOfType(typeof(Spawner));
        spawner = (Spawner) spawners[0];
       StartCoroutine(AnimateText());
    }
    IEnumerator AnimateText()
    {
        wave.text = "0";
        int round = 0;
        yield return new WaitForSeconds(.7f);
        while(round < spawner.wavesSurvived)
        {
            round++;
            wave.text = round.ToString();
            yield return new WaitForSeconds(.05f);
        }
    }
    public void Continue()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
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

