using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        Destroy(other);
        if (other.gameObject.name == "Enemy")
        {
            
        }
    }
}
