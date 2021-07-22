using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{

    [SerializeField] float fieldOfViewAngle = 110f;             // Number of degrees, centred on forward, for the enemy see.
    public float attackRange= 1.5f;
    public bool playerInAttackRange = false;                          // Whether or not the player is currently sighted.
    public bool playerInSight = false;                          // Whether or not the player is currently sighted.

    public Vector3 personalLastSighting = Vector3.zero;
    public Vector3 lastHeard = Vector3.zero;

    private Vector3 resetPosition = Vector3.zero;

    //  References
    NavMeshAgent nav = null;                                    // Reference to the NavMeshAgent component.
    SphereCollider col = null;                                  // Reference to the sphere collider trigger component.
    Animator anim = null;                                       // Reference to the Animator.
    GameObject player = null;                                   // Reference to the player.
    Animator playerAnim = null;                                 // Reference to the player's animator component.
    CapsuleCollider playerCaps = null;                                 // Reference to the player's animator component.
    HashIDs hash = null;                                        // Reference to the HashIDs.

    Vector3 previousSighting = Vector3.zero;                    // Where the player was sighted last frame.                                      

    void Awake ()
    {
        // Setting up the references.
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Tags.player);

        playerAnim = player.GetComponent<Animator>();
        playerCaps = player.GetComponent<CapsuleCollider>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        resetPosition = transform.position;
        // Set the personal sighting and the previous sighting to the reset position.
        personalLastSighting = resetPosition;
        lastHeard = resetPosition;
    }
	
	void Update ()
    {
        anim.SetBool(hash.playerInAttackRangeBool, playerInAttackRange);
    }

    void OnTriggerStay (Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == player)
        {
            // By default the player is not in sight.
            playerInAttackRange = false;
            playerInSight = false;

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            print("detectRange");
            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                // ... and if a raycast towards the player hits something...
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    // ... and if the raycast hits the player...
                    if (hit.collider.gameObject == player)
                    {
                        print("see");
                        // ... the player is in sight.
                        playerInSight = true;


                        // Set the last global sighting is the players current position.
                        personalLastSighting = playerCaps.transform.position;
                        Vector3 sightingDeltaPos = personalLastSighting - transform.position;
                        if(sightingDeltaPos.sqrMagnitude <= attackRange*attackRange){
                            playerInAttackRange = true;                            
                        }

                    }
                }
            }

            // If the player is not in sight but is running or is SHOOTING...
            if (playerInSight== false){
                if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Crouching"))
                {
                    // ... and if the player is within hearing range...
                    if (CalculatePathLength(playerCaps.transform.position) <= col.radius)
                    {
                        // ... set the last personal sighting of the player to the player's current position.
                        lastHeard = playerCaps.transform.position;
                        print("heard");
                    }
                }
                if (playerAnim.GetCurrentAnimatorStateInfo(1).IsName("WeaponShoot"))
                {
                    Vector3 sightingDeltaPos = playerCaps.transform.position - transform.position;
                    // ... and if the player is within hearing range...                        
                    if(sightingDeltaPos.sqrMagnitude <= col.radius*col.radius){
                        // ... set the last personal sighting of the player to the player's current position.
                        personalLastSighting = playerCaps.transform.position;
                        // ... just to refresh the timer
                        playerInSight = true;
                        print("heardshot");
                    }
                }
            }
            
        }
    }

    void OnTriggerExit (Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == player)
        {
            // ... the player is not in sight.
            playerInAttackRange = false;
            playerInSight = false;
        }
    }


    float CalculatePathLength (Vector3 targetPosition)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}