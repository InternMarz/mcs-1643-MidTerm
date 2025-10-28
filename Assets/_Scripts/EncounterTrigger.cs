using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    [Header("Chance")]
    [Range(0f, 1f)]
    public float encounterChance = 0.4f; // 40%

    [Header("Display Switching")]
    public Camera mainCamera;           // usually your normal gameplay camera
    public Camera encounterCamera;      // camera to switch to for the encounter
    public GameObject encounterUI;      // optional canvas or UI to enable on encounter

    [Header("Collision Settings")]
    public string playerTag = "Player";
    public string encounterTag = "Encounter";
    public bool requireTrigger = true; // set true if you want OnTriggerEnter, false to use OnCollisionEnter

    private void Reset()
    {
        // Attempt to auto-assign main camera
        if (mainCamera == null && Camera.main != null) mainCamera = Camera.main;
    }

    // Use trigger collisions by default (common for encounter zones)
    private void OnTriggerEnter(Collider other)
    {
        if (!requireTrigger) return;
        TryStartEncounter(other.gameObject);
    }

    // Use physics collisions if configured that way
    private void OnCollisionEnter(Collision collision)
    {
        if (requireTrigger) return;
        TryStartEncounter(collision.gameObject);
    }

    private void TryStartEncounter(GameObject other)
    {
        if (other.CompareTag(playerTag) && CompareTag(encounterTag))
        {
            float roll = Random.value; // 0.0 - 1.0
            if (roll < encounterChance);
   
            else
            {
                // No encounter — you can add alternate behavior here if desired.
                Debug.Log("Encounter roll failed (" + roll + ")");
            }
        }
    
        // Switch displays: disable main camera, enable encounter camera, enable UI
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (encounterCamera != null) encounterCamera.gameObject.SetActive(true);
        if (encounterUI != null) encounterUI.SetActive(true);

        Debug.Log("Encounter started: switched displays.");
    }

    // Optional: method to end encounter and restore display
    public void EndEncounter()
    {
        if (encounterCamera != null) encounterCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (encounterUI != null) encounterUI.SetActive(false);
        Debug.Log("Encounter ended: restored displays.");
    }
}
