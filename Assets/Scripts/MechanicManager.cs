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

    GameObject mechanic;

    public Queue<Task> tasks;

    private void Start()
    {
        tasks = new Queue<Task>();
    }

    public Task GetTask()
    {
        if (tasks.Count > 0)
        {
            return tasks.Dequeue();
        }
        else
        {
            return null;
        }
    }

    public void AddTask(Task t)
    {
        tasks.Enqueue(t);
    }
}
