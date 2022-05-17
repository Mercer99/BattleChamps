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
            other.gameObject.GetComponent<CharacterStats>().TakeDamage(spikeDamage, 6, true); //dmg when collided with the spikes
            other.GetComponent<KnockbackReceiver>().Knockback(gameObject, 20, true, "SPIKED");
        }
        Debug.Log("Dmg taken");
    }
}
