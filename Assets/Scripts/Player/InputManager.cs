using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class InputManager : MonoBehaviour
{
    #region Variables
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    PauseManager pauseManager;
    PlayerUI playerUI; 

    public Vector2 movementInput;
    public float moveAmount; 
    public float vertInput;
    public float horiInput;

    public bool sprint_Input;
    public bool jump_Input;
    public bool sneak_Input;
    public bool attackRight_Input;
    public bool attackLeft_Input;
    public bool pause_Input; 
    #endregion
    
    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
        pauseManager = GetComponent<PauseManager>();
        playerUI = GetComponent<PlayerUI>(); 
    }

    #region Enable Disable
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            #region Player Actions
            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Sneak.performed += i => sneak_Input = true;
            playerControls.PlayerActions.Sneak.canceled += i => sneak_Input = false;
            playerControls.PlayerActions.AttackRight.performed += i => attackRight_Input = true;
            playerControls.PlayerActions.AttackRight.canceled += i => attackRight_Input = false;
            playerControls.PlayerActions.AttackLeft.performed += i => attackLeft_Input = true;
            playerControls.PlayerActions.AttackLeft.canceled += i => attackLeft_Input = false;
            playerControls.PlayerActions.Pause.performed += i => pause_Input = true;
            #endregion
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable(); 
    }
    #endregion

    public void HandleAllInputs()
    {
        HandlePauseInput();
        HandleMovementInput();
        HandleJumpingInput();
        HandleSneakInput();
        HandleAttackLeftInput();
        HandleAttackRightInput();
        if (playerLocomotion.isSneaking)
            return;
        if (playerLocomotion.isAttackLeft || playerLocomotion.isAttackRight)
            return;
        HandleSprintInput();
        
    }

    #region Input Types
    private void HandleMovementInput ()
    {
        vertInput = movementInput.y;
        horiInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horiInput) + Mathf.Abs(vertInput));
        animatorManager.UpdateAnimateMovementValues(0f, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintInput ()
    {
        if (sprint_Input && moveAmount > 0.5f && playerUI.playerCurrentStam >= 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false; 
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleSneakInput()
    {
        if (sprint_Input)
            return; 
        if (!sneak_Input)
        {
            playerLocomotion.isSneaking = false;
            animatorManager.animator.SetBool("isSneaking", false);
        }
        else if (sneak_Input)
            playerLocomotion.HandleSneaking();
        else
            return; 
    }

    private void HandleAttackLeftInput ()
    {
        if (sprint_Input)
            return;
        if (!attackLeft_Input)
        {
            playerLocomotion.isAttackLeft = false;
            animatorManager.animator.SetBool("isAttackLeft", false);
        }
        else if (attackLeft_Input)
        {
            playerLocomotion.HandleAttackLeft();
        }
        else
            return; 
    }

    private void HandleAttackRightInput()
    {
        if (sprint_Input)
            return;
        if (!attackRight_Input)
        {
            playerLocomotion.isAttackRight = false;
            animatorManager.animator.SetBool("isAttackRight", false);
        }
        else if (attackRight_Input)
        {
            playerLocomotion.HandleAttackRight();
        }
        else
            return;
    }

    private void HandlePauseInput()
    {
        if (!pause_Input)
        {
            pauseManager.isPaused = false;
        }
        else if (pause_Input)
        {
            pause_Input = false;
            pauseManager.isPaused = true;
            pauseManager.PauseGame(); 
        }
    }
    #endregion
}
