using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;

    [Header("Properties")]
    public float travelSpeed;
    public float projectileDamage;

    void Update()
    {
        if (target == null)
        {
            DestroyProjectile();
        }
        else
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            HurtTarget();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, travelSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }

    private void HurtTarget()
    {
        EnemyTesterScript e = target.GetComponent<EnemyTesterScript>();
        e.TakeDamage(projectileDamage); //The damage command of on the enemy object
        DestroyProjectile();
    }
}
