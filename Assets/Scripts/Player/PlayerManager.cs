using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerUI playerUI; 
    Animator animator;

    public bool isInteracting;

    public LayerMask enemyLayer; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        playerUI = GetComponent<PlayerUI>(); 
    }

    #region Update Functions
    private void Update()
    {
        inputManager.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }
    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        playerLocomotion.isSneaking = animator.GetBool("isSneaking"); 
        animator.SetBool("isGrounded", playerLocomotion.isGrounded); 
    }
    #endregion
    public void TakeDmg()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;

        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.forward, out hit, enemyLayer))
        {
            print("DMG");
            playerUI.playerCurrentHP -= 1f * Time.deltaTime;
        }
        else
            print("Sucks");
    }
}
