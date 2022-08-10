using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public InputManager inputManager;
    public PlayerManager playerManager;
    public EnemyController enemyController;

    public float playerMaxHP = 100f;
    public float playerCurrentHP = 100f;

    public float playerMaxStam = 100f;
    public float playerCurrentStam = 100f;

    public Image fillImageHP;
    public Image fillImageStam;
    public Slider healthBar;
    public Slider stamBar;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        enemyController = GetComponent<EnemyController>();
    }
    void Update()
    {
        float fillValueHP = playerCurrentHP / playerMaxHP;
        float fillValueStam = playerCurrentStam / playerMaxStam; 
        healthBar.value = fillValueHP;
        stamBar.value = fillValueStam; 
        HealthCap();
        HealthDrain(); 
        StaminaCap();
        StamDrain();
    }

    public void HealthDrain()
    {
        //playerLo.TakeDmg();
        print(playerCurrentHP);
    }

    //Stam Drain while sprinting
    public void StamDrain()
    {
        if (inputManager.moveAmount <= 0.1f)
        {
            playerCurrentStam += 0.1f;
            return; 
        }

        if (inputManager.sprint_Input)
        {
            playerCurrentStam -= 0.1f; 
        }
        else
        {
            playerCurrentStam += 0.1f;
        }
    }
    //Current HP can't go over Max HP// Neither can be negative
    public void HealthCap()
    {
        if (playerMaxHP <= 0f)
        {
            playerMaxHP = 0f; 
        }
        if (playerCurrentHP <= 0f)
        {
            playerCurrentHP = 0f;
        }

        if (playerCurrentHP >= playerMaxHP)
        {
            playerCurrentHP = playerMaxHP; 
        }
    }
    //Current Stam can't go over Max Stam// Neither can be negative
    public void StaminaCap()
    {
        if (playerMaxStam <= 0f)
        {
            playerMaxStam = 0f;
        }
        if (playerCurrentStam <= 0f)
        {
            playerCurrentStam = 0f;
        }

        if (playerCurrentStam >= playerMaxStam)
        {
            playerCurrentStam = playerMaxStam;
        }
    }

}
