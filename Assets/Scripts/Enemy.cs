﻿using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameManager gm;
    public Bank bank;

    [Tooltip("Money paid to the player when this enemy dies")]
    public int money;

    [Tooltip("Amount of damage this enemy can take before it dies")]
    public float health;

    [Tooltip("Movement speed")]
    public float speed;

    [Tooltip("Current waypoint this enemy should move towards")]
    protected Transform targetWaypoint;
    protected int waypointIndex;
    public Waypoints waypoints;

    protected bool hasDied;

    private void Start(){
        waypointIndex = 0;
        targetWaypoint = waypoints.waypoints[waypointIndex];
        hasDied = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    protected void Move()
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
            gm.DecrementBy(1);
            Die();
        }
    }

    private void GetNextWaypoint()
    {
        targetWaypoint = waypoints.GetNextWaypoint(++waypointIndex);
    }

    private void Die()
    {
        if (!hasDied)
        {
            hasDied = true;
            Spawner.enemiesAlive--;
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            bank.DepositMoney(money);
            Die();
        }
    }
}
