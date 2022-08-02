using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PauseManager : MonoBehaviour
{

    public bool isPaused;
    public GameObject pauseMenu;
    public GameObject playerUI; 

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            playerUI.SetActive(false); 
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            playerUI.SetActive(true);
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
