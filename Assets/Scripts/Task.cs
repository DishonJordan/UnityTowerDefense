using UnityEngine;

public class Task
{
    public enum Status
    {
        Waiting,
        InProgress,
        Finished
    }

    public enum Type
    {
        Build,
        Sell,
        Upgrade,
        Repair
    }

    public Vector3 taskLocation;
    public GameObject mechanic;
    public Status status;
    private Type type;
    private MonoBehaviour script;
    private GameObject turretToBuild;

    public Task(Vector3 loc, Type t, MonoBehaviour s, GameObject tb)
    {
        taskLocation = loc;
        type = t;
        script = s;
        turretToBuild = tb;
        status = Status.Waiting;
    }

    public void UpdateStatus(Status s)
    {
        status = s;
    }

    public void AssignMechanic(GameObject m)
    {
        mechanic = m;
    }

    public void PerformTask()
    {
        switch (type)
        {
            case Type.Build:
                BuildManager buildManager = (BuildManager)script;
                buildManager.BuildTurret(turretToBuild);
                break;
            case Type.Sell:
                Turret t1 = (Turret)script;
                t1.SellTurret();
                break;
            case Type.Upgrade:
                Turret t2 = (Turret)script;
                t2.SellTurret();
                break;
            case Type.Repair:
                Turret t3 = (Turret)script;
                t3.SellTurret();
                break;
            default:
                break;
        }
    }
}