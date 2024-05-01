using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_AnimationController : MonoBehaviour
{
    private Animator animator;
    private Dragon_PlayerControl playerControls;
    private bool isInteracting;

    private void Awake(){
        animator = GetComponent<Animator>();
        playerControls = GetComponent<Dragon_PlayerControl>();
    }

    private void OnEnable(){
        playerControls.OnMove += HandleMove;
        Dragon_PlayerMovements.successFullInteraction += HandleInteract;
        Dragon_PlayerMovements.interactionClosed += HandleTakeOff;
    }

    private void OnDisable(){
        playerControls.OnMove -= HandleMove;
        Dragon_PlayerMovements.successFullInteraction -= HandleInteract;
        Dragon_PlayerMovements.interactionClosed -= HandleTakeOff;
    }

    private void HandleInteract(){
        isInteracting = true;
        animator.SetTrigger("TouchDown");
    }

    private void HandleTakeOff(){
        Debug.Log("TakeOff");
        isInteracting = false;
        animator.SetTrigger("Take Off");
    }

    private void HandleMove(Vector2 input){
        if(isInteracting){
            return;
        }

        Debug.Log("Handle Move" + input);
        if(input == Vector2.zero){
            Debug.Log("HandleMove Idle");
            animator.SetTrigger("Idle01");
        }
        else if(Mathf.Abs(input.x) > 0.2f){
            animator.SetTrigger(input.x > 0? "Fly Float": "Fly Float");
            Debug.Log("HandleMove RightLeft");
        }
        else if(input.y >0.1f){
            animator.SetTrigger("Run");
            Debug.Log("HandleMove Forward");
        }

    }
}
