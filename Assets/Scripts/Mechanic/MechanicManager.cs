﻿using UnityEngine;
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

    [Header("Task Elements")]
    public GameObject taskButton;
    public GameObject taskPanel;

    [Header("Mechanic")]
    public Material highlightColor;

    private LinkedList<Task> tasks;
    private Dictionary<GameObject, Task> taskDict;
    private GameObject spawnLocation;
    private Color originalColor;

    private void Start()
    {
        tasks = new LinkedList<Task>();
        taskDict = new Dictionary<GameObject, Task>();
        spawnLocation = transform.GetChild(3).gameObject;
        originalColor = GetComponent<MeshRenderer>().materials[1].color;

        Instantiate(mechanic, spawnLocation.transform.position, spawnLocation.transform.rotation);
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
        if (!mechanicTaskUI.activeSelf)
        {
            GetComponent<MeshRenderer>().materials[1].color = highlightColor.color;
        }
    }

    /* Remove hover highlight effect */
    public void OnMouseExit()
    {
        if (!mechanicTaskUI.activeSelf)
        {
            GetComponent<MeshRenderer>().materials[1].color = originalColor;
        }
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
    public void RemoveTask(Task t)
    {
        LinkedListNode<Task> node = tasks.Find(t);
        if (node != null)
        {
            tasks.Remove(node);
        }
    }

    private Task GetTaskFromButton(GameObject button)
    {
        if (taskDict.TryGetValue(button, out Task t))
        {
            return t;
        }
        return null;
    }

    public void RemoveTaskFromQueue(GameObject button)
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
    }
}
