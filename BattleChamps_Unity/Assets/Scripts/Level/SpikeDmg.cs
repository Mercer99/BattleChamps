using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDmg : MonoBehaviour
{
    public float spikeDamage = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(spikeDamage, gameObject); //dmg when collided with the spikes
        }
        Debug.Log("Dmg taken");
    }
}
