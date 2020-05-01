using UnityEngine;

public class ProjectileCatapult : Projectile
{
    private Vector3 startPosition;
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
            float x0 = startPosition.x;
            float x1 = target.transform.position.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, travelSpeed * Time.deltaTime);
            float baseY = Mathf.Lerp(startPosition.y, target.transform.position.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

            transform.position = nextPos;
        }
    }
}
