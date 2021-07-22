using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] float deadZone = 4f;              // The number of degrees for which the rotation isn't controlled by Mecanim.

    Transform player = null;                           // Reference to the player's transform.
    EnemySight enemySight = null;                      // Reference to the EnemySight script.
    NavMeshAgent nav = null;                           // Reference to the nav mesh agent.
    Animator anim = null;                              // Reference to the Animator.
    HashIDs hash = null;                               // Reference to the HashIDs script.
    AnimatorSetup animSetup = null;                    // An instance of the AnimatorSetup helper class.
    public int health = 5;
    
    EnemyShooting canAttack = null;
    EnemyAI eAI = null;
    bool dead = false;
    AudioSource audioSource = null;
    public List<AudioClip> zombieShot;
    public AudioClip fleshSound;
    public List<AudioClip> zombieKilled;
    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        audioSource = GetComponent<AudioSource>();
        // Making sure the rotation is controlled by Mecanim.
        nav.updateRotation = false;

        // Creating an instance of the AnimatorSetup class and calling it's constructor.
        animSetup = new AnimatorSetup(anim, hash);

        // We need to convert the angle for the deadzone from degrees to radians.
        deadZone *= Mathf.Deg2Rad;
        canAttack=GetComponent<EnemyShooting>();
        enemySight = GetComponent<EnemySight>();
        eAI = GetComponent<EnemyAI>();
    }
	
	void Update ()
    {
        // Calculate the parameters that need to be passed to the animator component.
        if (!dead){
            if(health<=0){
                if (anim.GetCurrentAnimatorStateInfo(0).IsTag("dying"))
                {
                    anim.SetBool("dead", false);
                    anim.SetBool(hash.playerInAttackRangeBool, false);
                }
            }else{
                NavAnimSetup();
            }            
        }
    }

    public void gotHit(string hitboxTag){
        if(audioSource.isPlaying)
            audioSource.Stop();        
        switch (hitboxTag){
            case Tags.hitbox_head:
                health-=4;
                break;
            case Tags.hitbox_body:
                health-=2;
                break;
            case Tags.hitbox_leg:
                health-=1;
                break;
            case Tags.hitbox_arm:
                health-=1;
                break;
        }
        if(health<=0){
            //zombie dies
            nav.isStopped=true;
            animSetup.Setup(0, 0);
            canAttack.enabled=false;
            enemySight.enabled=false;
            eAI.enabled=false;
            anim.SetBool("dead",true);
            print("zombie dead");
            audioSource.PlayOneShot(zombieKilled[Random.Range(0, zombieKilled.Count)],1);
            audioSource.PlayOneShot(fleshSound,1);            
        }else{
            anim.SetTrigger("hit");
            print(health);
            audioSource.PlayOneShot(zombieShot[Random.Range(0, zombieShot.Count)],1);
            audioSource.PlayOneShot(fleshSound,1);
        }
    }
    //  Affects the Root Motion manually
    void OnAnimatorMove ()
    {
        // Set the NavMeshAgent's velocity to the change in position since the last frame, by the time it took for the last frame.
        nav.velocity = anim.deltaPosition / Time.deltaTime;

        // The gameobject's rotation is driven by the animation's rotation.
        transform.rotation = anim.rootRotation;
    }

    void NavAnimSetup ()
    {
        // Create the parameters to pass to the helper function.
        float speed;
        float angle;

            // Otherwise the speed is a projection of desired velocity on to the forward vector...
        speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;

            // ... and the angle is the angle between forward and the desired velocity.
        angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
            //  To avoid snake movement between navigation points...
            // If the angle is within the deadZone...
        if (Mathf.Abs(angle) < deadZone)
        {
             // ... set the direction to be along the desired direction and set the angle to be zero.
            transform.LookAt(transform.position + nav.desiredVelocity);
            angle = 0f;
        }

        // Call the Setup function of the helper class with the given parameters.
        animSetup.Setup(speed, angle);
    }

    //  Returns angle calculated between both vectors in radians
    float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        // If the vector the angle is being calculated to is 0...
        if (toVector == Vector3.zero)
        {
            // ... the angle between them is 0.
            return 0f;
        }

        // Create a float to store the angle between the facing of the enemy and the direction it's travelling.
        float angle = Vector3.Angle(fromVector, toVector);

        // Find the cross product of the two vectors (this will point up if the velocity is to the right of forward).
        Vector3 normal = Vector3.Cross(fromVector, toVector);

        // The dot product of the normal with the upVector will be positive if they point in the same direction.
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));

        // We need to convert the angle we've found from degrees to radians.
        angle *= Mathf.Deg2Rad;

        return angle;
    }
}