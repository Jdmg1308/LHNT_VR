using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RobotAIScript : MonoBehaviour
{
    public bool dead = false;
    public bool isGrabbed = false;
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
    public Transform bulletSpawnPoint;
    float shootCooldown = 3.0f;
    public float aggroRange = 8.0f;

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
        if(dead) return;
        if(isPatrolling){
            Patrol();
        }else{
            if(playerTarget != null){
                LookAndShootAtTarget();
            }
        }
        
    }

    void LookAndShootAtTarget(){
        if (playerTarget == null || isGrabbed) return;

        if(Vector3.Distance(transform.position, playerTarget.position) > aggroRange){
            SetToPatrolling();
            return;
        }

        anim.SetFloat("Speed", 0);

        // Get direction to target
        // Debug.Log("transform name : " + transform.name);
        // Debug.Log("playerTarget.position: " + playerTarget.position);
        Vector3 directionToTarget = playerTarget.position - transform.position;
        directionToTarget.y = 0; // Ensure rotation happens only on the Y-axis

        // Get the angle between forward direction and target direction
        float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

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
        } else {
            anim.SetBool("turnLeft", false);
            anim.SetBool("turnRight", false);

            // Shoot at target)
            if(shootCooldown <= 0){
                anim.SetBool("shoot", true);
                StartCoroutine(StartDelay());
                shootCooldown = 2.0f;
            }else{
                anim.SetBool("shoot", false);
                shootCooldown -= Time.deltaTime;
            }
        }
    }



    void Patrol(){
        if(agent.remainingDistance < agent.stoppingDistance){
            if(patrolType == PatrolPathScript.PatrolType.Loop){
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
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

    public void SetToNotPatrolling(Transform target)
    {
        Debug.Log("Player Detected");
        isPatrolling = false;

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        agent.enabled = false;

        playerTarget = target;
    }


    public void SetToPatrolling()
    {
        isPatrolling = true;

        agent.enabled = true;

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        playerTarget = null;
    }


    IEnumerator StartDelay(){
        yield return new WaitForSeconds(.5f);
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * 5;

    }

    public void OnXRSelect()
    {
        isGrabbed = true;
        agent.enabled = false; // FULLY disable agent
        anim.SetBool("grabbed", true);
        anim.SetBool("shoot", false);
        anim.SetBool("turnLeft", false);
        anim.SetBool("turnRight", false);
    }

    public void OnXRRelease()
    {
        isGrabbed = false;
        agent.enabled = true; // RE-ENABLE agent
        anim.SetBool("grabbed", false);
        anim.SetBool("shoot", false);
        anim.SetBool("turnLeft", false);
        anim.SetBool("turnRight", false);

        if (patrolPoints != null && patrolPoints.Count > 0)
            agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);

        playerTarget = null;
        isPatrolling = true;
    }

    [ContextMenu("Launch Robot")]
    public void LaunchRobot() {
        // Launch Robot same direction as player is looking
        Vector3 launchDirection = Camera.main.transform.forward;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; 
        rb.AddForce(launchDirection * 10f, ForceMode.Impulse); 
        rb.AddTorque(Vector3.up * 5f, ForceMode.Impulse); 
        rb.useGravity = true;
        anim.SetBool("die", true);
        dead = true;
        Invoke("WaitForDie", 2.0f);
        transform.Find("Rob03").GetComponent<XRGrabInteractable>().enabled = false;
    }

    void WaitForDie(){
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        agent.enabled = false;
    } 
}
