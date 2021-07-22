using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 2f;
    public float health = 2f;                         // How much health the player has left.
    public float resetAfterDeathTime = 5f;              // How much time from the player dying to the level reseting.
    public AudioClip deathClip = null;                  // The sound effect of the player dying.
    public AudioClip healClip = null;
    public Text textHealth;      // health number display and bar


    Animator anim = null;                               // Reference to the animator component.
    FirstPersonAIO playerMovement = null;               // Reference to the player movement script.
    HashIDs hash = null;                                // Reference to the HashIDs.
    SceneFadeInOut sceneFadeInOut = null;               // Reference to the SceneFadeInOut script.
    float timer = 0f;                                   // A timer for counting to the reset of the level once the player is dead.
    bool playerDead = false;                            // A bool to show if the player is dead or not.

    //AudioSource audioSource;

    void Awake ()
    {
        // Setting up the references.
        //audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        playerMovement = GetComponent<FirstPersonAIO>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();   

 
    }


	void Update ()
    {   UpdateDisplay();
        // If health is less than or equal to 0...
        if (health <= 0f)
        {
            // ... and if the player is not yet dead...
            if (!playerDead)
            {
                // ... call the PlayerDying function.
                PlayerDying();
            }
            else {
                // Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
                PlayerDead();
                LevelReset();
            }
        }
    }

    void PlayerDying ()
    {
        // The player is now dead.
        playerDead = true;

        // Set the animator's dead parameter to true also.
        anim.SetBool(hash.deadBool, playerDead);

        // Play the dying sound effect at the player's location.
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    void PlayerDead ()
    {
        // If the player is in the dying state then reset the dead parameter.
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.dyingState)
        {
            anim.SetBool(hash.deadBool, false);
        }

        // Disable the movement.
        anim.SetFloat(hash.fowardFloat, 0f);
        playerMovement.enabled = false;

        // Stop the footsteps playing.
        //audioSource.Stop();
    }

    void LevelReset ()
    {
        // Increment the timer.
        timer += Time.deltaTime;

        //If the timer is greater than or equal to the time before the level resets...
        if (timer >= resetAfterDeathTime)
        {
            // ... reset the level.
            sceneFadeInOut.EndScene();
        }
    }


    public void TakeDamage (float damageAmount)
    {
        // Decrement the player's health by amount.
        health -= damageAmount;
    }


       public void Heal(float healAmount){
	if (health + healAmount> maxHealth ){
	   health = maxHealth;
	}
	else
	{
	health += healAmount;
	}
	AudioSource.PlayClipAtPoint(healClip, transform.position);
        }

/*

	public void OnCollisionEnter(Collision collision){
	if (collision.gameObject.GetComponent<Aid>() && health<maxHealth){	
	Heal(1f);
	Destroy(collision.gameObject);
        }
	}
*/
	void UpdateDisplay(){
	     textHealth.text = "Health: " + health.ToString();
	}

	
    public void UseMedicine()
    {
        int cost = 50;
        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (health < maxHealth && inventory.money >= cost)
        {
            inventory.money -= cost;
            health = maxHealth;
           // imageHealthbar.fillAmount = health / healthMax;
        }
    }




}
