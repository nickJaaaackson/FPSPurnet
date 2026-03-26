using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;

    public event Action JumpEvent;
    public event Action InteractEvent;
    public event Action PreviousEvent;
    public event Action NextEvent;

    public event Action<bool> AttackEvent;
    public event Action<bool> CrouchEvent;
    public event Action<bool> SprintEvent;

    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool PreviousPressed { get; private set; }
    public bool NextPressed { get; private set; }

    public bool AttackPressed { get; private set; }
    public bool CrouchPressed { get; private set; }
    public bool SprintPressed { get; private set; }

    private Controls controls;

    /// <summary>
    /// Initialize input actions and enable the player action map.
    /// </summary>
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    /// <summary>
    /// Disable the player action map.
    /// </summary>
    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Disable();
        }
    }

    /// <summary>
    /// Read movement input continuously.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(MovementInput);
    }

    /// <summary>
    /// Read look input continuously.
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        LookEvent?.Invoke(LookInput);
    }

    /// <summary>
    /// Register a one-shot jump input.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpPressed = true;
            JumpEvent?.Invoke();
        }
    }

    /// <summary>
    /// Register attack hold input.
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            AttackPressed = true;
            AttackEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            AttackPressed = false;
            AttackEvent?.Invoke(false);
        }
    }

    /// <summary>
    /// Register a one-shot interact input.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractPressed = true;
            InteractEvent?.Invoke();
        }
    }

    /// <summary>
    /// Register crouch hold input.
    /// </summary>
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            CrouchPressed = true;
            CrouchEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            CrouchPressed = false;
            CrouchEvent?.Invoke(false);
        }
    }

    /// <summary>
    /// Register a one-shot previous selection input.
    /// </summary>
    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PreviousPressed = true;
            PreviousEvent?.Invoke();
        }
    }

    /// <summary>
    /// Register a one-shot next selection input.
    /// </summary>
    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            NextPressed = true;
            NextEvent?.Invoke();
        }
    }

    /// <summary>
    /// Register sprint hold input.
    /// </summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            SprintPressed = true;
            SprintEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            SprintPressed = false;
            SprintEvent?.Invoke(false);
        }
    }

    /// <summary>
    /// Reset one-shot jump input after it has been consumed.
    /// </summary>
    public void ResetJumpInput()
    {
        JumpPressed = false;
    }

    /// <summary>
    /// Reset one-shot interact input after it has been consumed.
    /// </summary>
    public void ResetInteractInput()
    {
        InteractPressed = false;
    }

    /// <summary>
    /// Reset one-shot previous input after it has been consumed.
    /// </summary>
    public void ResetPreviousInput()
    {
        PreviousPressed = false;
    }

    /// <summary>
    /// Reset one-shot next input after it has been consumed.
    /// </summary>
    public void ResetNextInput()
    {
        NextPressed = false;
    }

    /// <summary>
    /// Reset all one-shot button inputs.
    /// </summary>
    public void ResetOneShotInputs()
    {
        JumpPressed = false;
        InteractPressed = false;
        PreviousPressed = false;
        NextPressed = false;
    }

    /// <summary>
    /// Consume jump input and clear it immediately.
    /// </summary>
    public bool ConsumeJump()
    {
        bool value = JumpPressed;
        JumpPressed = false;
        return value;
    }

    /// <summary>
    /// Consume interact input and clear it immediately.
    /// </summary>
    public bool ConsumeInteract()
    {
        bool value = InteractPressed;
        InteractPressed = false;
        return value;
    }

    /// <summary>
    /// Consume previous input and clear it immediately.
    /// </summary>
    public bool ConsumePrevious()
    {
        bool value = PreviousPressed;
        PreviousPressed = false;
        return value;
    }

    /// <summary>
    /// Consume next input and clear it immediately.
    /// </summary>
    public bool ConsumeNext()
    {
        bool value = NextPressed;
        NextPressed = false;
        return value;
    }
}