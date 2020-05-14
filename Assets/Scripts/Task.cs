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
    private Sprite turretIcon;
    private int cost;

    /* Location of task, Type of task, Script to execute task, 
     * optional gameobject to be used as a parameter for task execution, 
     * and a icon for the turret */
    public Task(Vector3 loc, Type t, MonoBehaviour s, GameObject tb, Sprite icon, int c)
    {
        taskLocation = loc;
        type = t;
        script = s;
        turretToBuild = tb;
        turretIcon = icon;
        cost = c;
    }

    /* Runs the associated function on the script */
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

    /* Gets the Name of the Task type*/
    public string GetTaskName()
    {
        switch (type)
        {
            case Type.Build:
                return "Build";
            case Type.Sell:
                return "Sell";
            case Type.Upgrade:
                return "Upgrade";
            case Type.Repair:
                return "Repair";
            default:
                return "ERROR";
        }
    }

    /* Gets the Turret Sprite */
    public Sprite GetIcon()
    {
        return turretIcon;
    }

    /* Refunds player task money and un-highlights task */
    public void CancelTask()
    {
        switch (type)
        {
            case Type.Build:
                ((BuildManager)script).UndoPendingTask();
                Bank.instance.DepositMoney(cost);
                break;
            case Type.Sell:
                ((Turret)script).UndoPendingTask();
                break;
            case Type.Upgrade:
                ((Turret)script).UndoPendingTask();
                Bank.instance.DepositMoney(cost);
                break;
            case Type.Repair:
                ((Turret)script).UndoPendingTask();
                Bank.instance.DepositMoney(cost);
                break;
            default:
                break;
        }
    }
}
