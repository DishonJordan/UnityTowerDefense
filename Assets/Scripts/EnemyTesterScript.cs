using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesterScript : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 15f), step);
    }
}
