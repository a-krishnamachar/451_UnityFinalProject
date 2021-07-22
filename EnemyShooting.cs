using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] float Damage = 1f;                     // The minimum potential damage per shot.
    Animator anim = null;                                           // Reference to the animator.
    HashIDs hash = null;                                            // Reference to the HashIDs script.
    SphereCollider col = null;                                      // Reference to the sphere collider.
    Transform player = null;                                        // Reference to the player's transform.
    PlayerHealth playerHealth = null;                               // Reference to the player's health.
    EnemySight enemySight = null;                           // Reference to the EnemySight script.
    bool shooting = false;                                          // A bool to say whether or not the enemy is currently shooting.
    public List<AudioClip> attackSound;
    AudioSource audioSource = null;

    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        enemySight = GetComponent<EnemySight>();
        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        // Cache the current value of the shot curve.
        float shot = anim.GetFloat(hash.attackFloat);

        // If the shot curve is peaking and the enemy is not currently shooting...
        if (shot > 0.5f && !shooting)
        {
            // ... shoot
            Shoot();
        }

        // If the shot curve is no longer peaking...
        if (shot < 0.5f)
        {
            // ... the enemy is no longer shooting and disable the line renderer.
            shooting = false;
        }
    }

    void Shoot()
    {
        // The enemy is shooting.
        shooting = true;

        Vector3 sightingDeltaPos = player.position - transform.position;
                        
        if(sightingDeltaPos.sqrMagnitude <= enemySight.attackRange*enemySight.attackRange){
            playerHealth.TakeDamage(Damage);
        }

        // The player takes damage.

        // Display the shot effects.
        ShotEffects();
    }

    void ShotEffects ()
    {
        if(audioSource.isPlaying)
            audioSource.Stop();                
        // zombie attack sound
        audioSource.PlayOneShot(attackSound[Random.Range(0, attackSound.Count)],1);
    }
}
