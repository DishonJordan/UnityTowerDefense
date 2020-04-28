using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PauseUI;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseUI();
        }
    }
    public void TogglePauseUI()
    {
        PauseUI.SetActive(!PauseUI.activeSelf);
        if(PauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
    }
    public void Retry()
    {
        TogglePauseUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        Debug.Log("Go to Menu"); // TODO
    }
}
