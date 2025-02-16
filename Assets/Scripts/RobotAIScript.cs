using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAIScript : MonoBehaviour
{
    public PatrolPathScript patrolPathScript;
    List<Transform> patrolPoints;
    NavMeshAgent agent;
    public int currentPatrolPointIndex = 0;
    PatrolPathScript.PatrolType patrolType;
    Animator anim;
    // Start is called before the first frame update
    void Start() {
        patrolType = patrolPathScript.patrolType;
        patrolPoints = new List<Transform>(patrolPathScript.patrolPoints);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", agent.speed);
    }

    // Update is called once per frame
    void Update() {
        if(agent.remainingDistance < agent.stoppingDistance){
            if(patrolType == PatrolPathScript.PatrolType.Loop){
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
            }else if(patrolType == PatrolPathScript.PatrolType.PingPong){
                if(currentPatrolPointIndex == patrolPoints.Count - 1){
                    currentPatrolPointIndex = 0;
                    patrolPoints.Reverse();
                }else{
                    currentPatrolPointIndex++;
                }
                agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
            }
        }
    }
}
