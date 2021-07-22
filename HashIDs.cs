using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour
{
    // Here we store the hash tags for various strings used in our animators.
    public int dyingState = 0;
    public int crouchingState = 0;
    public int isShootingState = 0;
    public int deadBool = 0;
    public int speedFloat = 0;//zombies
    public int fowardFloat = 0;//players
    //public int sneakingBool = 0;
    public int playerInAttackRangeBool = 0;
    public int attackFloat = 0;
    //public int aimWeightFloat = 0;
    public int angularSpeedFloat = 0;
    //public int openBool = 0;

    void Awake ()
    {
        dyingState = Animator.StringToHash("Base.Dying");
        crouchingState = Animator.StringToHash("Crouching");
        isShootingState = Animator.StringToHash("Shooting.WeaponShoot");
        
        deadBool = Animator.StringToHash("Dead");
        speedFloat = Animator.StringToHash("Speed");
        fowardFloat = Animator.StringToHash("Forward");
        //sneakingBool = Animator.StringToHash("Sneaking");

        playerInAttackRangeBool = Animator.StringToHash("playerInAttackRange");
        attackFloat = Animator.StringToHash("attack");
        //aimWeightFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        //openBool = Animator.StringToHash("Open");
    }
}