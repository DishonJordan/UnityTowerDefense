using UnityEngine;
using System.Collections.Generic;
using TMPro;

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

    private void Start()
    {
        tasks = new LinkedList<Task>();
    }

    public Task GetTask(Transform parent)
    {
        if (tasks.Count > 0)
        {
            Task t = tasks.First.Value;
            tasks.RemoveFirst();

            Transform b = taskPanel.transform.GetChild(0);
            b.SetParent(parent);
            b.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;

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

        GameObject b = Instantiate(taskButton, taskPanel.transform);
        b.GetComponentInChildren<TextMeshProUGUI>().SetText(t.GetTaskName());
        b.transform.SetParent(taskPanel.transform);
    }

    public void OnMouseDown()
    {
        /* Toggle Mechanic UIs */
        mechanicUpgradeUI.SetActive(!mechanicUpgradeUI.activeSelf);
        mechanicTaskUI.SetActive(!mechanicTaskUI.activeSelf);
    }
}
