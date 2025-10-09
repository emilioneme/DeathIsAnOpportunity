using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHub : MonoBehaviour
{
    // === Continuous values ===
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    // === Jump states ===
    public bool JumpHeld { get; private set; }
    public bool JumpPressed { get; private set; }

    // === Attack states ===
    public bool AttackHeld { get; private set; }
    public bool AttackPressed { get; private set; }

    // === Interact ===
    public bool InteractPressed { get; private set; }

    // === Events (optional, nice for one-shots) ===
    public event System.Action OnJumpPressed;
    public event System.Action OnAttackPressed;
    public event System.Action OnInteractPressed;
    public event System.Action OnFingerPressed;

    // --- Input System callbacks ---
    public void OnFinger(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) OnFingerPressed?.Invoke();   
    }
    public void OnMove(InputAction.CallbackContext ctx) 
        => Move = ctx.ReadValue<Vector2>();

    public void OnLook(InputAction.CallbackContext ctx) 
        => Look = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            JumpPressed = true;
            JumpHeld = true;
            OnJumpPressed?.Invoke();
        }
        else if (ctx.canceled)
        {
            JumpHeld = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            AttackPressed = true;
            AttackHeld = true;
            OnAttackPressed?.Invoke();
        }
        else if (ctx.canceled)
        {
            AttackHeld = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerUI playerUI = GetComponent<PlayerUI>();
            InteractPressed = true;
            // OnInteractPressed?.Invoke();
            
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    playerUI.interacted = true;
                    //Debug.Log("Interaction noted");
                    interactable.Interact();
                }
            }
        }
    }

    void LateUpdate()
    {
        // Reset "pressed" flags at end of frame so they're one-frame only
        JumpPressed = false;
        AttackPressed = false;
        InteractPressed = false;
    }

    void OnDisable()
    {
        // Reset all states so they don’t stick if input disables
        Move = Look = Vector2.zero;
        JumpHeld = JumpPressed = false;
        AttackHeld = AttackPressed = false;
        InteractPressed = false;
    }
}


