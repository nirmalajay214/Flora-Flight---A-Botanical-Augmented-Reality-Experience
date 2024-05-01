using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Dragon_PlayerControl : MonoBehaviour
{
    
    
    public event Action<Vector2> OnMove = delegate {};
    public event Action<Vector2> OnRise = delegate {};
    public event Action OnInterAct = delegate {};

    private DragonInputActions playerInput;

    private InputAction moveAction;
    private InputAction riseAction;
    private InputAction interAction;

    private void Awake(){
        playerInput = new DragonInputActions();
        moveAction = playerInput.DragonActionMaps.MoveForwardSide;
        riseAction =playerInput.DragonActionMaps.MoveUpDown;
        interAction = playerInput.DragonActionMaps.Interact;
    }

    private void OnEnable(){
        playerInput.Enable();
        moveAction.performed += Move_performed;
        moveAction.canceled += Move_performed;

        riseAction.performed += RisePerformed;
        riseAction.canceled += RisePerformed;

        interAction.performed += InterActionPerformed;
    }

    private void OnDisable(){
        playerInput.Disable();
        moveAction.performed -= Move_performed;
        moveAction.canceled -= Move_performed;

        riseAction.performed -= RisePerformed;
        riseAction.canceled -= RisePerformed;
    }

    private void Move_performed(InputAction.CallbackContext ctx){
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void RisePerformed(InputAction.CallbackContext ctx){
        OnRise?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void InterActionPerformed(InputAction.CallbackContext ctx){
        OnInterAct?.Invoke();
    }

    void ExternalInteractionPerformed(){
        OnInterAct?.Invoke();
    }
    
}
