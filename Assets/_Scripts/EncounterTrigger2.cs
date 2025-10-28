using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterTrigger2 : MonoBehaviour
{
    public float encounterChance = 0.4f; // 40%
    private void OnTriggerEnter(Collider other)
    {
       float roll = Random.value; // 0.0 - 1.0
        if (other.transform.CompareTag("Player")&&roll<encounterChance)
        {
            SceneManager.LoadScene("BattleScene");
        }
        
    }
}
