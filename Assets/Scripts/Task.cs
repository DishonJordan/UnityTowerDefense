using UnityEngine;

public abstract class Task
{
    public enum Type
    {
        Build,
        Sell,
        Upgrade,
        Repair
    }

    public Vector3 taskLocation;
    protected Type type;
    protected MonoBehaviour script;
    protected Sprite turretIcon;
    protected int cost;

    public Task(Vector3 taskLoc, Type taskType, MonoBehaviour taskScript, Sprite taskTurretIcon, int taskCost)
    {
        taskLocation = taskLoc;
        type = taskType;
        script = taskScript;
        turretIcon = taskTurretIcon;
        cost = taskCost;
    }

    /* Runs the associated function on the script */
    public abstract void PerformTask();

    /* Gets the Name of the Task type*/
    public abstract string GetTaskName();

    /* Gets the Turret Sprite */
    public Sprite GetIcon()
    {
        return turretIcon;
    }

    /* Refunds player task money and un-highlights task */
    public abstract void CancelTask();
}

public class BuildTask : Task
{

    private GameObject turretToBuild;

    public BuildTask(Vector3 taskLoc, Type taskType, MonoBehaviour taskScript,
        GameObject taskTurretToBuild, Sprite taskTurretIcon, int taskCost)
        : base(taskLoc, taskType, taskScript, taskTurretIcon, taskCost)
    {
        turretToBuild = taskTurretToBuild;
    }

    public override void PerformTask()
    {
        ((BuildManager)script).BuildTurret(turretToBuild);
    }

    public override string GetTaskName()
    {
        return "Build";
    }

    public override void CancelTask()
    {
        ((BuildManager)script).SetTaskActive(false);
        Bank.instance.DepositMoney(cost);
    }
}

public class SellTask : Task
{
    public SellTask(Vector3 taskLoc, Type taskType, MonoBehaviour taskScript,
        Sprite taskTurretIcon, int taskCost)
        : base(taskLoc, taskType, taskScript, taskTurretIcon, taskCost)
    {
    }

    public override void PerformTask()
    {
        ((Turret)script).SellTurret();
    }

    public override string GetTaskName()
    {
        return "Sell";
    }

    public override void CancelTask()
    {
        ((Turret)script).SetTaskActive(false);
    }
}

public class UpgradeTask : Task
{
    public UpgradeTask(Vector3 taskLoc, Type taskType, MonoBehaviour taskScript,
        Sprite taskTurretIcon, int taskCost)
        : base(taskLoc, taskType, taskScript, taskTurretIcon, taskCost)
    {
    }

    public override void PerformTask()
    {
        ((Turret)script).UpgradeTurret();
    }

    public override string GetTaskName()
    {
        return "Upgrade";
    }

    public override void CancelTask()
    {
        ((Turret)script).SetTaskActive(false);
        Bank.instance.DepositMoney(cost);
    }
}

public class RepairTask : Task
{
    public RepairTask(Vector3 taskLoc, Type taskType, MonoBehaviour taskScript,
        Sprite taskTurretIcon, int taskCost)
        : base(taskLoc, taskType, taskScript, taskTurretIcon, taskCost)
    {
    }

    public override void PerformTask()
    {
        ((Turret)script).RepairTurret();
    }

    public override string GetTaskName()
    {
        return "Repair";
    }

    public override void CancelTask()
    {
        ((Turret)script).SetTaskActive(false);
        Bank.instance.DepositMoney(cost);
    }
}
