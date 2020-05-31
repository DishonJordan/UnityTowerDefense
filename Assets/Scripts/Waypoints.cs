using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List <Transform> waypoints;
    public float lengthOfPath;

    [ContextMenu("Generate Path")]
    private void GenPath(){
        List <Transform> newWaypoints = new List <Transform> ();
        int count = transform.childCount;
        int curCount = 1;
        bool hasStarted = false;
        for(int i = 0; i < count; i++){
            var x = transform.GetChild(i);
            if(x.name == "Start"){
                newWaypoints.Add(x.transform);
                hasStarted = true;
            }
            else if(x.name == "End"){
                newWaypoints.Add(x.transform);
                hasStarted = false;
            }
            else if(hasStarted){
                x.name = "Waypoint " + curCount.ToString();
                newWaypoints.Add(x.transform);
                curCount++;
            }
        }

        if(newWaypoints.Count == 0){
            Debug.LogError("No Start waypoint found!");
            return;
        }
        if(hasStarted){
            Debug.LogError("No End waypoint found!");
            return;
        }

        waypoints = newWaypoints;
        lengthOfPath = 0;
        for(int i = 1; i < waypoints.Count; i++){
            lengthOfPath += Vector3.Distance(waypoints[i-1].position, waypoints[i].position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 x = new Vector3(0.25f,0,-0.25f);
        Vector3 y = new Vector3(-0.25f,0,-0.25f);
        for(int i = 0; i < waypoints.Count - 1; i++){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            // Gizmos.DrawLine(waypoints[i + 1].position, waypoints[i + 1].position + x);
            // Gizmos.DrawLine(waypoints[i + 1].position, waypoints[i + 1].position + y);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(waypoints[0].position, 0.5f);
        Gizmos.DrawWireSphere(waypoints[waypoints.Count - 1].position, 0.5f);
    }

    public Transform GetNextWaypoint(int index){
        if(index < waypoints.Count){
            return waypoints[index];
        }
        else{
            return null;
        }
    }
}
