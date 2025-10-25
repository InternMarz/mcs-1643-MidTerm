using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Ensure the game starts unpaused
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);

            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
            }
        }
    }

    public void OnClick()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}

