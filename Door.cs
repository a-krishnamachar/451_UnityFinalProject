using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    public bool keyRequired = false;
    PlayerInventory inventory = null;
    public bool isopen = false;
    void Awake()
    {
        animator = GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerInventory>();
    }
    private void Update() {
        if (animator.GetBool("open")!=isopen){
            animator.SetBool("open",isopen);
        }
    }

    // Update is called once per frame
    public void Interact(){
        if((!keyRequired) || (keyRequired && inventory.hasKey)){       
            isopen=!isopen;     
        }

    }
}
