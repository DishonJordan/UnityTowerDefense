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

    public Task(Type taskType, MonoBehaviour taskScript)
    {
        type = taskType;
        script = taskScript;
        taskLocation = taskScript.transform.position;
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

    /* Gets the Task Type */
    public Type GetTaskType() {
        return type;
    }

    /* Refunds player task money and un-highlights task */
    public abstract void CancelTask();
}

public class BuildTask : Task
{

    private GameObject turretToBuild;

    public BuildTask(MonoBehaviour taskScript, GameObject taskTurretToBuild)
        : base(Type.Build, taskScript)
    {
        turretToBuild = taskTurretToBuild;
        Turret t = taskTurretToBuild.GetComponentInChildren<Turret>();
        turretIcon = t.TurretSprite;
        cost = t.purchaseCost;

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
    public SellTask(MonoBehaviour taskScript)
        : base(Type.Sell, taskScript)
    {
        turretIcon = ((Turret)script).TurretSprite;
        cost = 0; // Not used by SellTask
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
    public UpgradeTask(MonoBehaviour taskScript)
        : base(Type.Upgrade, taskScript)
    {
        turretIcon = ((Turret)script).TurretSprite;
        cost = ((Turret)script).upgradeCost;
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
    public RepairTask(MonoBehaviour taskScript)
        : base(Type.Repair, taskScript)
    {
        turretIcon = ((Turret)script).TurretSprite;
        cost = ((Turret)script).repairCost;
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
