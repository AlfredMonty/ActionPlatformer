using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical"); 
    }

    public void PlayTargetAnim (string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting); 
        animator.CrossFade(targetAnim, 0.2f); 
    }

    public void UpdateAnimateMovementValues(float horizontalMovement, float verticalMovement, bool isMovement)
    {
        float snappedHori;
        float snappedVert;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHori = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHori = 1f; 
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHori = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHori = -1f;
        }
        else
        {
            snappedHori = 0f; 
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVert = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVert = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVert = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVert = -1f;
        }
        else
        {
            snappedVert = 0f;
        }
        #endregion

        if (isMovement)
        {
            snappedHori = horizontalMovement; 
            snappedVert = 2; 
        }

        animator.SetFloat(horizontal, snappedHori, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVert, 0.1f, Time.deltaTime);
    }
}
