using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;

    [Header("Properties")]
    public float travelSpeed;
    public float projectileDamage;


    private void Update()
    {
        /* If the target already died, then Destroy, else follow the target */
        if (target == null)
        {
            DestroyProjectile();
        }
        else
        {
            FollowTarget();
        }
    }

    /* Moves the projectile in the direction of the target */
    private void FollowTarget()
    {
        /* If the projectile gets close enough to the target, then deal damage to the target and destroy projectile */
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            HurtTarget();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, travelSpeed * Time.deltaTime);
        }
    }

    /* Setter for the target gameobject */
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    /* Performs the necessary steps the destroy the projectile*/
    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }

    /* Deals damage to the enemy and destroys the projectile */
    private void HurtTarget()
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable.TakeDamage(projectileDamage);
        DestroyProjectile();
    }
}
