using UnityEngine;

public class ProjectileCatapult : Projectile
{
    //WIP
    private Vector3 startPosition;
    private Vector3 targetPostion;
    public float arcHeight = 1;

    private void Start()
    {
        startPosition = transform.position;
    }

    protected override void FollowTarget()
    {
        /* If the projectile gets close enough to the target, then deal damage to the target and destroy projectile */
        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            HurtTarget();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, travelSpeed * Time.deltaTime);
        }
    }

    public override void SetTarget(GameObject target)
    {
        this.target = target;
        targetPostion = target.transform.position;
    }
}
