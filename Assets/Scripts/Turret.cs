using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject currentTarget;

    public int purchaseCost;
    public int sellCost;
    public int repairCost;

    public GameObject turretProjectile;

    public float fireRate;
    public float fireRange;

    private float turnRate = 10f;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
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

    void CheckForTargets()
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

    void TrackTarget()
    {
        if (currentTarget != null)
        {
            //Case where enemy walks out of turret range
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > fireRange)
            {
                currentTarget = null;
                return;
            }

            Vector3 direction = currentTarget.transform.position - transform.position;
            Quaternion qRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, qRotation, Time.deltaTime * turnRate).eulerAngles;

            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }


    void FireProjectile()
    {

        if (currentTarget != null)
        {
            GameObject projectile = Instantiate(turretProjectile, firePoint.position, firePoint.rotation);
            Projectile p = projectile.GetComponent<Projectile>();

            p.SetTarget(currentTarget);
        }

    }

    /* When clicking on the turret in the scene, it will show the fireRange of the turret */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
