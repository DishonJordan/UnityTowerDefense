using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    public Text text;
    private Spawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
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
            text.text = Mathf.Clamp(spawner.waveIndex + 1, 1, waveCount).ToString() + " / " + waveCount.ToString();
        }
        else
        {
            text.text = "No Wave";
        }
    }
}
