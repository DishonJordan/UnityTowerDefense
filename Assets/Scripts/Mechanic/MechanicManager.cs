using UnityEngine;
using System.Collections.Generic;

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

    private GameObject mechanic;
    private LinkedList<Task> tasks;

    private void Start()
    {
        tasks = new LinkedList<Task>();
    }

    public Task GetTask()
    {
        if (tasks.Count > 0)
        {
            Task t = tasks.First.Value;
            tasks.RemoveFirst();
            return t;
        }
        else
        {
            return null;
        }
    }

    public void AddTask(Task t)
    {
        tasks.AddLast(t);
    }

    public void OnMouseDown()
    {
        /* Toggle Mechanic UIs */
        mechanicUpgradeUI.SetActive(!mechanicUpgradeUI.activeSelf);
        mechanicTaskUI.SetActive(!mechanicTaskUI.activeSelf);
    }
}
