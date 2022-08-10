using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    #region Variables
    AnimatorManager animatorManager; 
    InputManager inputManager;
    PlayerManager playerManager;
    PlayerUI playerUI; 

    public Vector3 moveDir;
    Transform cameraObject;
    Rigidbody rb;

    [Header("Attack Types")]
    public bool isAttackRight;
    public bool isAttackLeft; 

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public float gravModifier = 1f; 
    public LayerMask groundLayer; 

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isSneaking;

    [Header("Move Speeds")]
    public float attackMoveSpeed = 0f; 
    public float sneakSpeed = 0.5f; 
    public float walkSpeed = 1.5f; 
    public float runSpeed = 5f;
    public float sprintSpeed = 7f; 
    public float rotationSpeed = 15f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3f;
    public float gravIntensity = -15f ;
    #endregion

    #region HandleAllMovement
    public void HandleAllMovement()
    {
        HandleFallAndLand();
        HandleRotation();
        if (playerManager.isInteracting)
            return;
        HandleMovement();
    }
#endregion

    #region FixedUpdate and Awake
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Physics.gravity * gravModifier, ForceMode.Acceleration);
    }

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        playerUI = GetComponent<PlayerUI>(); 
        cameraObject = Camera.main.transform;
    }
    #endregion

    #region All Handles

    //Movement based on input and camera diraction
    private void HandleMovement()
    {
        if (isJumping)
            return;
        moveDir = cameraObject.forward * inputManager.vertInput;
        moveDir = moveDir + cameraObject.right * inputManager.horiInput;
        moveDir.Normalize();
        moveDir.y = 0f;

        //Various speeeds being applied to moveDir
        if (isSneaking)
        {
            moveDir = moveDir * sneakSpeed;
        }

        if (isSprinting)
        {
            moveDir = moveDir * sprintSpeed;
        }
        else
        {
            if (isAttackLeft || isAttackRight)
            {
                moveDir = moveDir * attackMoveSpeed; 
            }
            else if (inputManager.moveAmount >= 0.5f)
            {
                moveDir = moveDir * runSpeed;
            }
            else
            {
                moveDir = moveDir * walkSpeed;
            }
        }
        if (isSneaking)
        {
            moveDir = moveDir * sneakSpeed;
        }
        moveDir = moveDir * runSpeed; 
        Vector3 movementVelo = moveDir;
        rb.velocity = new Vector3(movementVelo.x, rb.velocity.y, movementVelo.z);  
    }

    //Handles and locks rotation during specific events
    private void HandleRotation ()
    {
        if (isJumping)
            return;
        Vector3 targetDir = Vector3.zero;
        targetDir = cameraObject.forward * inputManager.vertInput;
        targetDir = targetDir + cameraObject.right * inputManager.horiInput;
        targetDir.Normalize();
        targetDir.y = 0f;

        if (targetDir == Vector3.zero)
            targetDir = transform.forward; 

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation; 
    }

    //Falling and landing detecting ground
    private void HandleFallAndLand()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset; 
        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnim("Falling", true); 
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * fallingVelocity * inAirTimer); 
        }
        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnim("Landing", true);
            }
            inAirTimer = 0f;
            isGrounded = true;
            playerManager.isInteracting = false;
        }
        else
        {
            isGrounded = false; 
        }
    }

    //Jumping w/ animation
    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", false);
            animatorManager.PlayTargetAnim("Jumping", true);

            float jumpingVelocity = Mathf.Sqrt(-2f * gravIntensity * jumpHeight);
            Vector3 playerVelocity = moveDir;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
     }

    //Sneaking w/ animation
    public void HandleSneaking()
    {
        if (isGrounded && !isSneaking)
        {
            animatorManager.animator.SetBool("isSneaking", true);
            isSneaking = true;
        }
    }

    //Handle attacks
    public void HandleAttackLeft()
    {
        if (isSprinting)
            return; 
        if (!isAttackLeft)
        {
            animatorManager.animator.SetBool("isAttackLeft", true);
            animatorManager.animator.SetBool("isAttackRight", false);
            isAttackLeft = true;
            isAttackRight = false; 
        }
    }
    public void HandleAttackRight()
    {
        if (isSprinting)
            return;
        if (!isAttackRight)
        {
            animatorManager.animator.SetBool("isAttackRight", true);
            animatorManager.animator.SetBool("isAttackLeft", false);
            isAttackLeft = false;
            isAttackRight = true;
        }
    }
    #endregion
}
