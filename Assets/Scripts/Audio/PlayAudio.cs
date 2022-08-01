using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource player; 
    public AudioClip punch;
    public AudioClip step;
    public AudioClip jump;
    public AudioClip land;

    InputManager inputManager;
    PlayerLocomotion playerLocomotion; 

    private void Awake()
    {
        player = GetComponent<AudioSource>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void PunchAudio()
    {
        if (inputManager.attackLeft_Input || inputManager.attackRight_Input)
        {
            player.clip = punch;
            player.Play();
        }
        else
            return; 
    }

    public void StepAudio()
    {
        if (playerLocomotion.isGrounded == false || player.clip == punch || player.clip == land)
            return; 
        player.clip = step;
        player.Play();
    }

    public void JumpAudio()
    {
        player.clip = jump;
        player.Play();
    }

    public void NullAudio()
    {
        player.clip = null; 
    }
}