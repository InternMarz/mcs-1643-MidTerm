using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterTrigger2 : MonoBehaviour
{
    public float encounterChance = 0.4f; // 40%
    public KeyCode shieldKey = KeyCode.LeftShift;
    private bool shieldActive = false;

    private void OnTriggerEnter(Collider other)
    {
        float roll = Random.value; // 0.0 - 1.0
        if (other.transform.CompareTag("Player") && roll < encounterChance)
        {
            SceneManager.LoadScene("BattleScene");
        }

    }
    void Update()
    {
        HandleShield();
    }

    void HandleShield()
    {
        if (Input.GetKey(shieldKey))
        {
            ActivateShield();
            encounterChance = 0f;
        }
        else
        {
            DeactivateShield();
            encounterChance = 0.4f;
        }
    }
    public void ActivateShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            encounterChance = 0f;
        }
    }

    public void DeactivateShield()
    {
        if (shieldActive)
        {
            shieldActive = false;
            encounterChance = 0.4f;
        }
    }
}
    
