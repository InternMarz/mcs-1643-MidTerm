using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceOtherScript : MonoBehaviour
{
    [Header("Reference to another script")]
    public MonoBehaviour targetScript;

    void Start()
    {
        if (targetScript != null)
        {
            Debug.Log("Referenced script: " + targetScript.GetType().Name);
        }
        else
        {
            Debug.LogWarning("No script referenced!");
        }
    }
}
