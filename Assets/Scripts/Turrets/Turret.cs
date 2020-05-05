﻿using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    public static bool turretUIActive;
    [Header("Name")]
    public string turretName;

    [Header("Costs")]
    public int purchaseCost;
    public int sellCost;
    public int repairCost;
    public int upgradeCost;

    [Header("Properties")]
    [Tooltip("After how many seconds does the turret shoots")]
    public float fireRate;
    [Tooltip("This range can be seen in the unity editor by clicking on the turret object")]
    public float fireRange;
    [Tooltip("The maximum health of this turret")]
    public float maxHealth;
    [Tooltip("The current health of this turret")]
    public float health;

    [Header("UI")]
    public GameObject turretUI;

    [Header("Misc")]
    public GameObject turretProjectile;
    public Transform firePoint;
    public GameObject nextUpgrade;

    protected GameObject currentTarget;
    private readonly float turnRate = 6f;
    private float timer;
    private BuildManager myTileBuildManager;

    /* Initializations that occur when the object is instantiated */
    private void Start()
    {
        timer = 0.0f;
    }

    /* Handles when the user clicks on a turret */
    private void OnMouseDown()
    {
        if (!turretUIActive && !BuildManager.shopUIActive)
        {
            EnableTurretUI();
        }
    }

    private void Update()
    {
        /* If there is no target, search for one, else track the target and coundown to shoot */
        if (currentTarget == null)
        {
            CheckForTargets();
        }
        else
        {
            TrackTarget();
            timer += Time.deltaTime;
            if (timer > fireRate)
            {
                FireProjectile();
                timer = 0.0f;
            }
        }
    }

    /* Searches through all gameobjects with the tage 'enemy' and sets the current target to the closest one within range */
    private void CheckForTargets()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float min_distance = Mathf.Infinity;
        GameObject closestGameObjectInRange = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < min_distance && distance < fireRange)
            {
                min_distance = distance;
                closestGameObjectInRange = enemy;
            }
        }
        currentTarget = closestGameObjectInRange;
    }

    /* Points the turret at the currentTarget */
    private void TrackTarget()
    {
        if (currentTarget != null)
        {
            /* If the current target goes out of range */
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > fireRange)
            {
                currentTarget = null;
                return;
            }

            Vector3 direction = currentTarget.transform.position - transform.position;
            Quaternion qRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, qRotation, Time.deltaTime * turnRate).eulerAngles;

            /* Rotates the turret about the y axis*/
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    /* Instantiates a projectile at the firepoint and sets the target of the projectile to the currentTarget */
    protected virtual void FireProjectile()
    {
        if (currentTarget != null)
        {
            GameObject projectile = Instantiate(turretProjectile, firePoint.position, firePoint.rotation);
            Projectile p = projectile.GetComponentInChildren<Projectile>();

            p.SetTarget(currentTarget);
        }
    }

    /* Interface for taking projectile damage from an enemy */
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            health = 0;
        }
    }

    /* Opens the turret UI */
    public void EnableTurretUI()
    {
        turretUI.SetActive(true);
        turretUIActive = true;
    }

    /* Closes the turret UI */
    public void DisableTurretUI()
    {
        turretUI.SetActive(false);
        turretUIActive = false;
    }

    /* Destroys the turrent, and refunds the player */
    public void SellTurret()
    {
        Bank.instance.DepositMoney(sellCost);
        DestroyTurret();
    }

    /* Upgrades the stats of the turret */
    public void UpgradeTurret()
    {
        if (nextUpgrade != null && Bank.instance.WithdrawMoney(upgradeCost))
        {
            /* Replaced turret on tile with the upgraded one */
            myTileBuildManager.ReplaceTurret(nextUpgrade);
            DestroyTurret();
        }
    }

    /* Repairs health of the turret */
    public void RepairTurret()
    {
        if(Bank.instance.WithdrawMoney(repairCost)){
            health += 25;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    /* Destroys the Current Turret */
    public void DestroyTurret()
    {
        DisableTurretUI();
        Destroy(this.transform.parent.gameObject);
    }

    /* Sets the BuildManager for the tile that this turret is on */
    public void SetBuildManager(BuildManager buildManager)
    {
        myTileBuildManager = buildManager;
    }

    /* When clicking on the turret in the scene, it will show the fireRange of the turret */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
