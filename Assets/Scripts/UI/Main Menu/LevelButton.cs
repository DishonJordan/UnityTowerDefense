using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Image thumbnail;
    public TextMeshProUGUI title;
    public int sceneId;

    public void PlayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }
}
