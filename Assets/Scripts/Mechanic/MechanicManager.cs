using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MechanicManager : MonoBehaviour
{
    #region Singleton Pattern

    public static MechanicManager instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    [Header("UI Elements")]
    public GameObject mechanicUpgradeUI;
    public GameObject mechanicTaskUI;

    [Header("Mechanic")]
    public GameObject mechanic;

    [Header("Upgrade Elements")]
    public GameObject upgradeButtonPanel;
    public float taskDecreaseUpgradeAmount;
    public float speedIncreaseUpgradeAmount;
    public Slider xpBar;
    public int currentUpgradeTreshold;

    [Header("Task Elements")]
    public GameObject taskButton;
    public GameObject taskPanel;

    [Header("Mechanic")]
    public Material highlightColor;

    private LinkedList<Task> tasks;
    private Dictionary<GameObject, Task> taskDict;
    private GameObject spawnLocation;
    private Color originalColor;
    private List<GameObject> mechanics;
    private readonly float spawnRadius = 0.175f;
    private int upgradesRemaining;

    private void Start()
    {
        tasks = new LinkedList<Task>();
        mechanics = new List<GameObject>();
        taskDict = new Dictionary<GameObject, Task>();

        spawnLocation = transform.GetChild(3).gameObject;
        originalColor = GetComponent<MeshRenderer>().materials[1].color;

        xpBar.maxValue = currentUpgradeTreshold;
        xpBar.value = 0;
        upgradesRemaining = 0;
        SetUpgradesActive(false);

        SpawnMechanic();
    }

    /* Toggle Mechanic UIs */
    public void OnMouseDown()
    {
        mechanicUpgradeUI.SetActive(!mechanicUpgradeUI.activeSelf);
        mechanicTaskUI.SetActive(!mechanicTaskUI.activeSelf);
        GetComponent<MeshRenderer>().materials[1].color = originalColor;
    }

    /* On hover highlight effect */
    public void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().materials[1].color = highlightColor.color;
    }

    /* Remove hover highlight effect */
    public void OnMouseExit()
    {
        GetComponent<MeshRenderer>().materials[1].color = originalColor;
    }

    /* Gets a new task from the Queue and removes it from the Queue UI */
    public Task GetTask()
    {
        if (tasks.Count > 0)
        {
            Task t = tasks.First.Value;
            tasks.RemoveFirst();

            GameObject b = taskPanel.transform.GetChild(0).gameObject;

            RemoveTask(t);
            taskDict.Remove(b);
            Destroy(b);

            return t;
        }
        else
        {
            return null;
        }
    }

    /* Removes a Task from the LinkedList */
    public void RemoveTask(Task t)
    {
        LinkedListNode<Task> node = tasks.Find(t);
        if (node != null)
        {
            tasks.Remove(node);
        }
    }

    /* Finds the task associated with a button in the taskDict */
    private Task GetTaskFromButton(GameObject button)
    {
        if (taskDict.TryGetValue(button, out Task t))
        {
            return t;
        }
        return null;
    }

    /* Removes the Task from the queue, deletes the button, and refunds user */
    public void DeleteTask(GameObject button)
    {
        Task t = GetTaskFromButton(button);

        if (t != null)
        {
            RemoveTask(t);
            taskDict.Remove(button);
            t.CancelTask();
            Destroy(button);
        }
    }

    /* Adds a Task to the Queue and Adds a button to the Queue UI */
    public void AddTask(Task t)
    {
        tasks.AddLast(t);

        GameObject b = Instantiate(taskButton, taskPanel.transform);
        b.GetComponentInChildren<TextMeshProUGUI>().SetText(t.GetTaskName());
        b.transform.GetChild(1).GetComponentInChildren<Image>().sprite = t.GetIcon();
        b.transform.SetParent(taskPanel.transform);

        Button button = b.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => DeleteTask(b));

        taskDict.Add(b, t);
    }

    /* Spawns a new mechanic at spawnLocation and adds the mechanic to the List of Mechanics */
    public void SpawnMechanic()
    {
        GameObject mech = Instantiate(mechanic, spawnLocation.transform.position +
            new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)),
            spawnLocation.transform.rotation);
        mechanics.Add(mech);

        upgradesRemaining = (upgradesRemaining - 1 < 0)? 0 : upgradesRemaining - 1;
        if (upgradesRemaining == 0)
        {
            SetUpgradesActive(false);
        }
    }

    /* Increases the Movement Speed of all Mechanics */
    public void IncreaseMovementSpeed()
    {
        foreach (GameObject mech in mechanics)
        {
            Mechanic m = mech.GetComponent<Mechanic>();
            m.IncreaseMovementSpeed(speedIncreaseUpgradeAmount);
        }

        upgradesRemaining--;
        if (upgradesRemaining == 0)
        {
            SetUpgradesActive(false);
        }
    }

    /* Increases the Movement Speed of all Mechanics */
    public void DecreaseTaskTime()
    {
        foreach (GameObject mech in mechanics)
        {
            Mechanic m = mech.GetComponent<Mechanic>();
            m.DecreaseTaskSpeed(taskDecreaseUpgradeAmount);
        }

        upgradesRemaining--;
        if (upgradesRemaining == 0) {
            SetUpgradesActive(false);
        }
    }

    /* Increases the Xp and adjust slider */
    public void IncreaseXp() {
        xpBar.value++;

        if (xpBar.value == currentUpgradeTreshold) {
            currentUpgradeTreshold *= 2;
            xpBar.maxValue = currentUpgradeTreshold;
            xpBar.value = 0;
            upgradesRemaining++;
            SetUpgradesActive(true);
        }
    }

    /* Enable or Disable the Upgrade Buttons */
    public void SetUpgradesActive(bool b) {
        Button[] buttons = upgradeButtonPanel.GetComponentsInChildren<Button>();

        foreach (Button button in buttons) {
            button.interactable = b;
        }
    }
}
