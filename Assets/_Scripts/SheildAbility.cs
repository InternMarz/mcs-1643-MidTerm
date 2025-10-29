using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShieldAbility : MonoBehaviour
{
    [Header("Shield Settings")]
    public KeyCode shieldKey = KeyCode.LeftShift; // The key to activate shield
    public GameObject shieldVisual;              // Prefab or sprite that shows in front of player
    public float normalSpeed = 1f;
    private float currentSpeed;

    [Header("References")]
    private RebindablePlayerMovement3D playerMovement; // Script controlling player movement

    private bool shieldActive = false;
    void Start()
    {
        playerMovement = GetComponent<RebindablePlayerMovement3D>();

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        currentSpeed = normalSpeed * 0.5f;
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
        }
        else
        {
            DeactivateShield();
        }
    }
    public void ActivateShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;

            // Lower player speed
            currentSpeed = normalSpeed * 0.5f;

            // Show shield in front of player
            if (shieldVisual != null)
                shieldVisual.SetActive(true);
        }
    }

    public void DeactivateShield()
    {
        if (shieldActive)
        {
            shieldActive = false;

            // Restore speed
            currentSpeed = normalSpeed;

            // Hide shield
            if (shieldVisual != null)
                shieldVisual.SetActive(false);
        }
    }
}
