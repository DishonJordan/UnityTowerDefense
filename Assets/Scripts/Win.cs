using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        // TODO Go to next level
    }
    public void Menu()
    {
        // TODO
    }
}

