using UnityEngine;

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
    private Transform targetWaypoint;
    private int waypointIndex;
    public Waypoints waypoints;
    public float distanceToEnd;
    public int strengthRating;
    public bool canSpawnOnDeath;
    public GameObject DeathSpawn;

    private bool hasDied;

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

    private void Move()
    {
        if(targetWaypoint != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetWaypoint.position);
            float distanceTraveledThisFrame = speed * Time.deltaTime;

            if(distanceTraveledThisFrame >= distanceToTarget)
            {
                transform.position = targetWaypoint.position;
                distanceToEnd -= Vector3.Distance(transform.position, targetWaypoint.position);
                GetNextWaypoint();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceTraveledThisFrame);
                distanceToEnd -= distanceTraveledThisFrame;
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

            if(canSpawnOnDeath){
                SpawnOnDeath();
            }

            var x = transform.Find("Explosion").gameObject;
            x = Instantiate(x,transform.position,transform.rotation);
            x.SetActive(true);
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
    
    private void SpawnOnDeath(){
        if(DeathSpawn == null){
            Debug.LogError("No enemy gameobject for spawning attached to this gameobject.");
            return;
        }

        for(int i = 0; i < 3; i++){
            GameObject newEnemy = Instantiate(DeathSpawn, transform.position + new Vector3(Random.Range(-0.3f,0.3f),0,Random.Range(-0.3f,0.3f)), transform.rotation);
            Enemy e = newEnemy.GetComponentInChildren<Enemy>();
            e.waypoints = waypoints;
            e.distanceToEnd = distanceToEnd;
            e.bank = bank;
            e.gm = gm;
            Waypoints w  = new Waypoints();
            w.waypoints = waypoints.waypoints.GetRange(waypointIndex, waypoints.waypoints.Count - waypointIndex);
            e.waypoints = w;
            Spawner.enemiesAlive++;
        }
    }
}
