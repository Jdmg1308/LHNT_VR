using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAIScript : MonoBehaviour
{
    public bool isPatrolling = true;
    public PatrolPathScript patrolPathScript;
    List<Transform> patrolPoints;
    NavMeshAgent agent;
    public int currentPatrolPointIndex = 0;
    PatrolPathScript.PatrolType patrolType;
    Animator anim;
    Transform playerTarget;
    public GameObject bulletPrefab;
    public float turnThreshold = 5.0f;

    public MeshCollider detectionCollider;
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
        if(isPatrolling){
            Patrol();
        }else{
            if(playerTarget != null){
                LookAndShootAtTarget();
            }
        }
        
    }

    void LookAndShootAtTarget(){
        if (playerTarget == null) return;

        anim.SetFloat("Speed", 0);

        // Get direction to target
        // Debug.Log("transform name : " + transform.name);
        // Debug.Log("playerTarget.position: " + playerTarget.position);
        Vector3 directionToTarget = playerTarget.position - transform.position;
        directionToTarget.y = 0; // Ensure rotation happens only on the Y-axis

        // Get the angle between forward direction and target direction
        float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);
        Debug.Log("angle: " + angle);

        // Determine turning direction
        if (Mathf.Abs(angle) > turnThreshold)
        {
            if (angle > 0)
            {
                anim.SetBool("turnLeft", true);
                anim.SetBool("turnRight", false);
            }
            else
            {
                anim.SetBool("turnLeft", false);
                anim.SetBool("turnRight", true);
            }
        }
        else
        {
            anim.SetBool("turnLeft", false);
            anim.SetBool("turnRight", false);
        }
    }



    void Patrol(){
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

    public void SetToNotPatrolling(Transform target){
        Debug.Log("Player Detected");
        isPatrolling = false;
        agent.isStopped = true;
        playerTarget = target;
        agent.enabled = false;
    }

    public void SetToPatrolling(){
        isPatrolling = true;
        agent.isStopped = false;
        playerTarget = null;
        agent.enabled = true;
    }
}
