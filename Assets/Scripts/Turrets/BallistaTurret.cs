using UnityEngine;

public class BallistaTurret : Turret
{
    private GameObject arrow;

    new void Start()
    {
        base.Start();
        arrow = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }

    /* Instantiates a projectile at the firepoint and sets the target of the projectile to the currentTarget */
    protected override void FireProjectile()
    {
        if (currentTarget != null)
        {
            arrow.SetActive(true);
            GameObject projectile = Instantiate(turretProjectile, firePoint.position, firePoint.rotation);
            Projectile p = projectile.GetComponent<Projectile>();

            p.SetTarget(currentTarget);
            arrow.SetActive(false);
        }
    }
}
