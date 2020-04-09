using UnityEngine;

public class EnemyTesterScript : MonoBehaviour
{
    public float speed;

    /* A simple script that moves the enemy, to test the range of the turret */
    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 15f), step);
    }

    /* Example enemy damage function */
    public void TakeDamage(float damage) {
        Destroy(this.gameObject);
    }
}
