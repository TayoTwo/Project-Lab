using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    public Animator animator;
    public bool moving;
    public bool interacting;
    public bool signaling;

    // Update is called once per frame
    void LateUpdate()
    {
        
        animator.SetBool("moving", moving);
        animator.SetBool("interacting", interacting);
        animator.SetBool("signaling",signaling);

    }


}