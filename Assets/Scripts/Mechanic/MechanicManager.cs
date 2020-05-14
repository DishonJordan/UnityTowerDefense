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

    public GameObject mechanicUpgradeUI;
    public GameObject mechanicTaskUI;

    public GameObject taskButton;
    public GameObject taskPanel;

    private GameObject mechanic;
    private LinkedList<Task> tasks;
    private Dictionary<GameObject,Task> taskDict;

    private void Start()
    {
        tasks = new LinkedList<Task>();
        taskDict = new Dictionary<GameObject, Task>();
        PrintDict();
    }

    /* Toggle Mechanic UIs */
    public void OnMouseDown()
    {
        mechanicUpgradeUI.SetActive(!mechanicUpgradeUI.activeSelf);
        mechanicTaskUI.SetActive(!mechanicTaskUI.activeSelf);
    }

    /* Gets a new task from the Queue and removes it from the Queue UI*/
    public Task GetTask()
    {
        if (tasks.Count > 0)
        {
            Task t = tasks.First.Value;
            tasks.RemoveFirst();

            GameObject b = taskPanel.transform.GetChild(0).gameObject;
            Destroy(b);

            return t;
        }
        else
        {
            return null;
        }
    }

    /* Removes a Task from the LinkedList */
    public void RemoveTask(Task t) {
        LinkedListNode<Task> node = tasks.Find(t);
        if (node != null) {
            tasks.Remove(node);
        }
    }

    private Task GetTaskFromButton(GameObject button) {
        if (taskDict.TryGetValue(button, out Task t)){
            return t;
        }
        return null;
    }

    public void RemoveTaskFromQueue(GameObject button) {
        Debug.Log("Attempting to Remove!");
        Task t = GetTaskFromButton(button);

        if (t != null) {
            Debug.Log("Found Task From Button");
            RemoveTask(t);
            taskDict.Remove(button);
            t.EndTask();
            Destroy(button);
        }
        PrintDict();
    }

    /* Adds a Task to the Queue and Adds a button to the Queue UI*/
    public void AddTask(Task t)
    {
        tasks.AddLast(t);

        GameObject b = Instantiate(taskButton, taskPanel.transform);
        b.GetComponentInChildren<TextMeshProUGUI>().SetText(t.GetTaskName());
        b.transform.GetChild(1).GetComponentInChildren<Image>().sprite = t.GetIcon();
        b.transform.SetParent(taskPanel.transform);

        Button button = b.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => RemoveTaskFromQueue(b));

        taskDict.Add(b, t);
        PrintDict();
    }

    private void PrintDict() {
        foreach (KeyValuePair<GameObject, Task> kvp in taskDict) {
            Debug.Log(kvp.Key + " " + kvp.Value);
        }
    }
}
