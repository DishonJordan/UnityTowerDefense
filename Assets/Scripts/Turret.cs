using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Costs")]
    public int purchaseCost;
    public int sellCost;
    public int repairCost;
    public int upgradeCost;

    [Header("Properties")]
    [Tooltip("After how many seconds does the turret shoots")]
    public float fireRate;
    [Tooltip("This range can be seen in the unity editor by clicking on the turet object")]
    public float fireRange;

    [Header("UI")]
    public GameObject turretUI;

    [Header("Misc")]
    public GameObject turretProjectile;
    public Transform firePoint;

    private GameObject currentTarget;
    private readonly float turnRate = 6f;
    private float timer;

    private void Start()
    {
        timer = 0.0f;
    }

    private void OnMouseDown()
    {
        EnableTurretUI();
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
    private void FireProjectile()
    {
        if (currentTarget != null)
        {
            GameObject projectile = Instantiate(turretProjectile, firePoint.position, firePoint.rotation);
            Projectile p = projectile.GetComponent<Projectile>();

            p.SetTarget(currentTarget);
        }

    }

    public void EnableTurretUI() {
        turretUI.SetActive(true);
    }

    public void DisableTurretUI() {
        turretUI.SetActive(false);
    }

    public void SellTurret() {
        Debug.Log("TODO: IMPLEMENT SELL");
    }

    public void UpgradeTurret() {
        Debug.Log("TODO: IMPLEMENT UPGRADE");
    }

    public void RepairTurret()
    {
        Debug.Log("TODO: IMPLEMENT REPAIR");
    }

    /* When clicking on the turret in the scene, it will show the fireRange of the turret */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
