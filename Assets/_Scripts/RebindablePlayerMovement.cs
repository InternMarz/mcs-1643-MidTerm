using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindablePlayerMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;            // Movement speed
    public float rotationSpeed = 10f;       // How quickly to rotate toward direction

    [Header("Key Bindings (can be changed in Inspector or at runtime)")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool waitingForKey = false;
    private Action<KeyCode> onKeyAssigned;

    // PlayerPrefs keys
    private const string PREF_FORWARD = "key_forward";
    private const string PREF_BACKWARD = "key_backward";
    private const string PREF_LEFT = "key_left";
    private const string PREF_RIGHT = "key_right";

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        LoadKeyBindings();
    }

    void Update()
    {
        // Read input for movement direction
        Vector3 input = Vector3.zero;

        if (Input.GetKey(forwardKey)) input.z += 1f;
        if (Input.GetKey(backwardKey)) input.z -= 1f;
        if (Input.GetKey(leftKey)) input.x -= 1f;
        if (Input.GetKey(rightKey)) input.x += 1f;

        moveInput = input.normalized;

        // Rotate toward movement direction (only if moving)
        if (moveInput.magnitude > 0.01f)
        {
            // Compute the desired rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);

            // Smoothly rotate toward target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Rebinding logic
        if (waitingForKey)
            DetectAnyKeyForRebind();
    }

    void FixedUpdate()
    {
        // Move using physics
        Vector3 velocity = moveInput * moveSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    #region Rebinding System

    void DetectAnyKeyForRebind()
    {
        foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kc))
            {
                waitingForKey = false;
                onKeyAssigned?.Invoke(kc);
                onKeyAssigned = null;
                break;
            }
        }
    }

    public void StartRebind(Action<KeyCode> assignCallback)
    {
        if (waitingForKey) return;
        waitingForKey = true;
        onKeyAssigned = assignCallback;
    }

    public void SaveKeyBindings()
    {
        PlayerPrefs.SetString(PREF_FORWARD, forwardKey.ToString());
        PlayerPrefs.SetString(PREF_BACKWARD, backwardKey.ToString());
        PlayerPrefs.SetString(PREF_LEFT, leftKey.ToString());
        PlayerPrefs.SetString(PREF_RIGHT, rightKey.ToString());
        PlayerPrefs.Save();
    }

    public void LoadKeyBindings()
    {
        if (PlayerPrefs.HasKey(PREF_FORWARD))
            forwardKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(PREF_FORWARD));

        if (PlayerPrefs.HasKey(PREF_BACKWARD))
            backwardKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(PREF_BACKWARD));

        if (PlayerPrefs.HasKey(PREF_LEFT))
            leftKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(PREF_LEFT));

        if (PlayerPrefs.HasKey(PREF_RIGHT))
            rightKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(PREF_RIGHT));
    }

    #endregion
}