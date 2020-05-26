using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    public Text nextWaveText;
    public Text waveTimerText;
    private Button CallWaveEarlyButton;
    private Spawner spawner;
    private Object[] spawners;
    void Awake () {
         CallWaveEarlyButton =  GameObject.Find ("CallWaveEarlyButton").GetComponent<Button>();
         nextWaveText = GameObject.Find ("NextWave").GetComponent<Text> ();
     }
    void Start()
    {
        spawners = GameObject.FindObjectsOfType(typeof(Spawner));
        spawner = (Spawner) spawners[0];
        nextWaveText.text = "START";
    }
    void Update()
    {
        

        if (spawner.wavesSurvived == 0)
        {
            waveTimerText.enabled = false;
            nextWaveText.text = "START";
        } else {
            waveTimerText.enabled = true;
            nextWaveText.text = "next\nwave in";
        }

        if(spawner.spawningWave || spawner.wavesSurvived == spawner.waves.Count)
        {
            waveTimerText.text = "0";
            CallWaveEarlyButton.interactable = false;
        } else 
        {
            waveTimerText.text = ((int) (spawner.delayBetweenWaves - spawner.waveTimer)).ToString();
            CallWaveEarlyButton.interactable = true;
        }
    }
}
