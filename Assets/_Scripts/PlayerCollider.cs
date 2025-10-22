using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            print("Player hit a wall!");
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            print("Player is touching a wall!");
        }
    }
}
