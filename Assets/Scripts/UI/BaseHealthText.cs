using UnityEngine;
using UnityEngine.UI;

public class BaseHealthText : MonoBehaviour
{
    public Text text;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("BaseHealthText could not find GameManager!");
        }
    }

    void Update()
    {
        if(gameManager != null)
        {
            text.text = gameManager.Health.ToString();
        }
        else
        {
            text.text = "No Base";
        }
    }
}
