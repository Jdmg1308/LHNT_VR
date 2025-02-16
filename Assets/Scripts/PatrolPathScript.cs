using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPathScript : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();

    public enum PatrolType
    {
        PingPong,
        Loop
    }

    public PatrolType patrolType;

    private void Start(){
        foreach (Transform child in transform){
            patrolPoints.Add(child.transform);
        }
    }

    // Show the patrol points in gizmos
    private void OnDrawGizmos(){
        patrolPoints.Clear();
        foreach (Transform child in transform){
            patrolPoints.Add(child.transform);
        }

        // Draw spheres for each patrol point first
        for(int i = 0; i < patrolPoints.Count; i++){
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(patrolPoints[i].position, 0.5f);
        }

        // Draw lines
        if(patrolType == PatrolType.PingPong){
            for(int i = 0; i < patrolPoints.Count - 1; i++){
                Gizmos.color = Color.green;
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
            Gizmos.color = Color.green;
        } else if(patrolType == PatrolType.Loop){
            for(int i = 0; i < patrolPoints.Count - 1; i++){
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(patrolPoints[patrolPoints.Count - 1].position, patrolPoints[0].position);
        }
    }
}