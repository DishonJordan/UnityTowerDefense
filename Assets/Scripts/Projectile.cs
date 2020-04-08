using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameObject target;

    public float travelSpeed;
    public float damage;


    // Update is called once per frame
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
        Debug.Log("THIS WILL DAMAGE TARGET");
        DestroyProjectile();
    }
}
