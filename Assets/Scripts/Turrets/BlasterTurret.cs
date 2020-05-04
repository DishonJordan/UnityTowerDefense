using UnityEngine;

public class BlasterTurret : Turret
{
    [Header("Blaster Extras")]
    public Transform firePoint2;

    private bool firePointSelector;

    /* Fires a projectile, alternating between Firepoint1 and 2 */
    protected override void FireProjectile()
    {
        if (currentTarget != null)
        {
            GameObject projectile = (firePointSelector) ? Instantiate(turretProjectile, firePoint.position, firePoint.rotation) :
                Instantiate(turretProjectile, firePoint2.position, firePoint2.rotation);

            Projectile p = projectile.GetComponent<Projectile>();
            p.SetTarget(currentTarget);

            firePointSelector = !firePointSelector;
        }
    }
}
