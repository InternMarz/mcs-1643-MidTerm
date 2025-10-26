using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
    [Header("Trigger Matching")]
    [Tooltip("If true, the other object will be matched by tag. Otherwise matched by exact name.")]
    public bool useTag = true;
    [Tooltip("Tag to match (when useTag = true). Example: 'Encounter'")]
    public string triggerTag = "Encounter";
    [Tooltip("Exact GameObject name to match (when useTag = false).")]
    public string triggerObjectName = "";

    [Header("Scene Loading")]
    public bool loadByName = true;
    [Tooltip("Exact scene name (when loadByName = true). Must be in Build Settings).")]
    public string sceneName = "";
    [Tooltip("Build index (when loadByName = false).")]
    public int sceneBuildIndex = 0;

    [Header("Behavior")]
    [Tooltip("Delay (seconds) before loading the scene. Useful to play an animation/sound first.")]
    public float loadDelay = 0f;
    [Tooltip("If true, only allow one scene load (prevents double-calls).")]
    public bool singleUse = true;

    bool hasTriggered = false;

    // ----------- Unity collision/trigger callbacks ------------

    // Called when this object collides with a non-trigger collider
    void OnCollisionEnter(Collision collision)
    {
        TryHandleCollision(collision.gameObject);
    }

    // Called when this object enters a trigger collider (isTrigger = true)
    void OnTriggerEnter(Collider other)
    {
        TryHandleCollision(other.gameObject);
    }

    // If you want this component on the "trigger" object instead of the Player,
    // the next two handlers will detect the Player specifically:
    void OnCollisionEnter2D(Collision2D collision) { } // placeholder if 2D physics used

    // ---------------- core logic -----------------

    void TryHandleCollision(GameObject other)
    {
        if (hasTriggered && singleUse) return;

        bool match = false;

        if (useTag && !string.IsNullOrEmpty(triggerTag))
        {
            if (other.CompareTag(triggerTag)) match = true;
        }
        else if (!string.IsNullOrEmpty(triggerObjectName))
        {
            if (other.name == triggerObjectName) match = true;
        }

        // Optional: only trigger when the "Player" object is involved.
        // If you attached this script to the Player, leave as-is (other is the other object).
        // If you attached to the trigger object and want to ensure only Player triggers it,
        // uncomment the following lines and adjust the playerTag as needed:
        //
        // string playerTag = "Player";
        // if (!other.CompareTag(playerTag)) return;

        if (match)
        {
            if (singleUse) hasTriggered = true;

            if (Mathf.Approximately(loadDelay, 0f))
            {
                LoadTargetScene();
            }
            else
            {
                StartCoroutine(LoadAfterDelay(loadDelay));
            }
        }
    }

    System.Collections.IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadTargetScene();
    }

    void LoadTargetScene()
    {
        if (loadByName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneChangeOnCollision] sceneName is empty. Set it in the inspector or use build index.");
                return;
            }
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}

