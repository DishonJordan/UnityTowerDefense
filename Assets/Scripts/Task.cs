using UnityEngine;

public class Task
{
    public enum Type
    {
        Build,
        Sell,
        Upgrade,
        Repair
    }

    public Vector3 taskLocation;
    private Type type;
    private MonoBehaviour script;
    private GameObject turretToBuild;

    /* Location of task, Type of task, Script to execute task, optional gameobject to be used as a parameter for task execution */
    public Task(Vector3 loc, Type t, MonoBehaviour s, GameObject tb)
    {
        taskLocation = loc;
        type = t;
        script = s;
        turretToBuild = tb;
    }

    public void PerformTask()
    {
        switch (type)
        {
            case Type.Build:
                ((BuildManager)script).BuildTurret(turretToBuild);
                break;
            case Type.Sell:
                ((Turret)script).SellTurret();
                break;
            case Type.Upgrade:
                ((Turret)script).UpgradeTurret();
                break;
            case Type.Repair:
                ((Turret)script).RepairTurret();
                break;
            default:
                break;
        }
    }
}