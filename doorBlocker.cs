using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBlocker : MonoBehaviour
{
    public bool isopen = false;
    Animator animator;
    public Door targetDoor;
    void Start()
    {        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("open")!=isopen){
            animator.SetBool("open",isopen);
        }        
    }
    public void Interact(){
        print(animator.GetBool("open"));
        if(!targetDoor.isopen){       
            isopen=!isopen;     
        }

    }    
}
