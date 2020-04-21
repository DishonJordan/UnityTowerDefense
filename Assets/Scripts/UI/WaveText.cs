using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    public Text text;
    // TODO: Add dependency to Spawner to obtain current wave index

    private void Start()
    {
        
    }

    private void Update()
    {
        text.text = "No Wave";
    }
}
