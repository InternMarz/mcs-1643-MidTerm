using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
   // public GameObject playerprefab;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverPanel.SetActive(true);
            //Time.timeScale = 0f; // Pause the game
        }
    }
}