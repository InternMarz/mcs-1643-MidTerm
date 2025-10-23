using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCameraSwitcher : MonoBehaviour
{
    public enum MatchMode { ByReference, ByTag }

    [Header("Collision match")]
    public MatchMode matchMode = MatchMode.ByReference;
    [Tooltip("Used when matchMode == ByReference")]
    public GameObject otherObjectReference;
    [Tooltip("Used when matchMode == ByTag")]
    public string otherObjectTag;

    [Header("Camera settings")]
    [Tooltip("Camera to switch to when the chance succeeds")]
    public Camera targetCamera;
    [Tooltip("If left blank, the script will try to find Camera.main as the original camera")]
    public Camera originalCamera;
    [Range(0f, 1f)]
    [Tooltip("Chance (0..1) to switch per collision. Default 0.25 = 25%")]
    public float switchProbability = 1f;

    [Header("UI settings")]
    [Tooltip("UI GameObject to enable when camera switches (e.g. a Canvas or Panel). Should be inactive by default.")]
    public GameObject uiToShow;

    [Header("Optional behavior")]
    [Tooltip("If > 0, camera will revert after this many seconds. Set 0 to never auto-revert.")]
    public float revertAfterSeconds = 5f;
    [Tooltip("Minimal time between processed collisions to avoid spamming (seconds)")]
    public float collisionCooldown = 0.5f;

    bool isUsingTargetCamera = false;
    float lastCollisionTime = -999f;

    void Awake()
    {
        if (originalCamera == null)
        {
            originalCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogWarning("[CollisionCameraSwitcher] targetCamera is not set.");
        }

        if (uiToShow != null)
        {
            uiToShow.SetActive(false); // start hidden
        }
    }

    // Use this if collider is NOT set to isTrigger
    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    // Use this if collider is set to isTrigger
    void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    void HandleCollision(GameObject collided)
    {
        // enforce cooldown
        if (Time.time - lastCollisionTime < collisionCooldown) return;
        lastCollisionTime = Time.time;

        // check match
        bool match = false;
        if (matchMode == MatchMode.ByReference)
        {
            match = otherObjectReference != null && collided == otherObjectReference;
        }
        else // ByTag
        {
            match = !string.IsNullOrEmpty(otherObjectTag) && collided.CompareTag(otherObjectTag);
        }

        if (!match) return;

        // probability check
        if (Random.value < switchProbability)
        {
            SwitchToTargetCamera();
        }
    }

    void SwitchToTargetCamera()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("[CollisionCameraSwitcher] No targetCamera assigned.");
            return;
        }

        if (isUsingTargetCamera) return; // already switched

        // disable original, enable target
        if (originalCamera != null) originalCamera.enabled = false;
        targetCamera.enabled = true;

        // show UI
        if (uiToShow != null) uiToShow.SetActive(true);

        isUsingTargetCamera = true;

        // optional revert
        if (revertAfterSeconds > 0f)
        {
            StartCoroutine(RevertAfterDelay(revertAfterSeconds));
        }
    }

    IEnumerator RevertAfterDelay(float secs)
    {
        yield return new WaitForSeconds(secs);
        RevertToOriginalCamera();
    }

    public void RevertToOriginalCamera()
    {
        if (!isUsingTargetCamera) return;

        if (targetCamera != null) targetCamera.enabled = false;
        if (originalCamera != null) originalCamera.enabled = true;

        if (uiToShow != null) uiToShow.SetActive(false);

        isUsingTargetCamera = false;
    }
}
