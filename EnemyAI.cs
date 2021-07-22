using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;public class EnemyAI : MonoBehaviour
{
    public List<AudioClip> idleSound;
    public List<AudioClip> patrolSound;
    public List<AudioClip> chaseSound;
    [SerializeField] float patrolSpeed = 2f;                // The nav mesh agent's speed when patrolling.
    [SerializeField] float chaseSpeed = 5f;                 // The nav mesh agent's speed when chasing.
    [SerializeField] float chaseWaitTime = 5f;              // The amount of time to wait when the last sighting is reached.
    EnemySight enemySight = null;                           // Reference to the EnemySight script.
    NavMeshAgent nav = null;                                // Reference to the nav mesh agent.
    //Transform player = null;                                // Reference to the player's transform.
    float chaseTimer = 6f;                                  // A timer for the chaseWaitTime.
    public float zombieVolume = 1f;
    
    AudioSource audioSource = null;
    void Awake ()
    {
        // Setting up the references.
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        audioSource = GetComponent<AudioSource>();        
    }

	void Update ()
    {
        if (enemySight.playerInSight || chaseTimer<chaseWaitTime) {
            // ... chase.
            Chasing();
        }

        // Otherwise...
        else {
            // ... patrol.
            Patrolling();
        }
    }

    void Chasing ()
    {
        // Create a vector from the enemy to the last sighting of the player.
        if (enemySight.playerInSight){
            chaseTimer = 0f;
        }
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        // If the the last personal sighting of the player is not close... else it is attacking
        nav.speed = chaseSpeed;
        
        if (sightingDeltaPos.sqrMagnitude > nav.stoppingDistance*nav.stoppingDistance)
        {
            // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
            print("chase me");
            nav.destination = enemySight.personalLastSighting;
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(chaseSound[Random.Range(0, chaseSound.Count)],zombieVolume);
        }

        // Set the appropriate speed for the NavMeshAgent.

            // ... increment the timer.
        chaseTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
        if (chaseTimer >= chaseWaitTime || Vector3.Distance(enemySight.personalLastSighting, transform.position)<1)
        {
                // ... reset last global sighting, the last personal sighting and the timer.
            enemySight.lastHeard = enemySight.personalLastSighting;
            chaseTimer+= chaseWaitTime;
        }
    }

    void Patrolling ()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        Vector3 heardDeltaPos = enemySight.lastHeard - transform.position;
        // Set an appropriate speed for the NavMeshAgent.
        //if havent reached lastHeard position, go
        if (sightingDeltaPos.sqrMagnitude> 0.3f || heardDeltaPos.sqrMagnitude>0.3f){
            print("patroll");
            nav.speed = patrolSpeed;
            nav.destination = enemySight.lastHeard;
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(patrolSound[Random.Range(0, patrolSound.Count)],zombieVolume);            
        }else{
            print("stopped");
            //nav.isStopped = true; This line stupifies it
            // maybe it would follow other zombies in the future?
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(idleSound[Random.Range(0, idleSound.Count)],zombieVolume);
        }
    }
}
