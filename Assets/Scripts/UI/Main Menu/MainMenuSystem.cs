using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSystem : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject rightPanel;
    public GameObject centerPanel;
    public GameObject levelSelectButtonPrefab;
    public Transform levelSelectButtonParent;
    public List<Level> levels;

    [System.Serializable]
    public class Level
    {
        public string title;
        public int sceneId;
        public Sprite thumbnail;
    }

    private void Start()
    {
        foreach(var level in levels)
        {
            GameObject buttonObj = Instantiate(levelSelectButtonPrefab, levelSelectButtonParent);
            LevelButton button = buttonObj.GetComponent<LevelButton>();
            button.thumbnail.sprite = level.thumbnail;
            button.title.text = level.title;
            button.sceneId = level.sceneId;
            buttonObj.SetActive(true);
        }
    }

    public void GotoMainMenu()
    {
        centerPanel.SetActive(false);
        rightPanel.SetActive(true);
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void GotoOptionsMenu()
    {
        centerPanel.SetActive(false);
        rightPanel.SetActive(true);
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void GotoLevelSelectMenu()
    {
        centerPanel.SetActive(true);
        rightPanel.SetActive(false);
    }
}
