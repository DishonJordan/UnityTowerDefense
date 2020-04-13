using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Tooltip("Money paid to the player when this enemy dies")]
    public int money;

    [Tooltip("Amount of damage this enemy can take before it dies")]
    public float health;

    [Tooltip("Movement speed")]
    public float speed;

    [Tooltip("Current waypoint this enemy should move towards")]
    private Transform targetWaypoint;
    private int waypointIndex;
    public GameObject waypoints;

    private void Start(){
        targetWaypoint = waypoints.GetComponent<Waypoints>().waypoints[0];
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(targetWaypoint != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position);
            float distanceTraveledThisFrame = speed * Time.deltaTime;

            if(distanceTraveledThisFrame >= distanceToTarget)
            {
                transform.position = targetWaypoint.position;
                GetNextWaypoint();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceTraveledThisFrame);
            }
        }
        else{
            Die();
        }
    }

    private void GetNextWaypoint()
    {
        targetWaypoint = waypoints.GetComponent<Waypoints>().GetNextWaypoint(++waypointIndex);
    }

    private void Die()
    {
        Debug.Log(this.gameObject.name + " died");
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }
}
