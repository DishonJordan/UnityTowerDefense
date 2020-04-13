using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] waypoints;

    private void OnDrawGizmosSelected()
    {
        Vector3 x = new Vector3(0.25f,0,-0.25f);
        Vector3 y = new Vector3(-0.25f,0,-0.25f);
        for(int i = 0; i < waypoints.Length - 1; i++){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            // Gizmos.DrawLine(waypoints[i + 1].position, waypoints[i + 1].position + x);
            // Gizmos.DrawLine(waypoints[i + 1].position, waypoints[i + 1].position + y);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(waypoints[waypoints.Length - 1].position, 0.5f);
    }

    public Transform GetNextWaypoint(int index){
        if(index < waypoints.Length){
            return waypoints[index];
        }
        else{
            return null;
        }
    }
}
