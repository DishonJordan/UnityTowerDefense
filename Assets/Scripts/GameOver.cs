using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public Text wave;
    private Spawner spawner;
    private Object[] spawners;
    void OnEnable()
    {
        spawners = GameObject.FindObjectsOfType(typeof(Spawner));
        spawner = (Spawner) spawners[0];
        wave.text = spawner.wavesSurvived.ToString();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

