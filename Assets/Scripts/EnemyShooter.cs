using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    private GameObject currentTarget;
    private float timer;
    public float fireRate;
    public float fireRange;
    private readonly float turnRate = 6f;
    public GameObject turretProjectile;
    public Transform firePoint;

    void Start()
    {
        waypointIndex = 0;
        targetWaypoint = waypoints.waypoints[waypointIndex];
        hasDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Move to waypoint
        Move();

        //Check for or fire at nearby turrets
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Turret");

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
            Projectile p = projectile.GetComponentInChildren<Projectile>();

            p.SetTarget(currentTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
