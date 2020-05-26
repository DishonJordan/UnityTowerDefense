using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    public Text text;
    private Spawner spawner;
    private Object[] spawners;
    private void Start()
    {
        spawners = GameObject.FindObjectsOfType(typeof(Spawner));
        spawner = (Spawner) spawners[0];
        if(spawner == null)
        {
            Debug.LogError("WaveText could not find Spawner!");
        }
    }

    private void Update()
    {
        if(spawner != null)
        {
            int waveCount = spawner.waves.Count;
            text.text = Mathf.Clamp(spawner.waveIndex , 0, waveCount).ToString() + " / " + waveCount.ToString();
        }
        else
        {
            text.text = "No Wave";
        }
    }
}
