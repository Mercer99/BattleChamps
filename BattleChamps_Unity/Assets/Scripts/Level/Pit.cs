using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float instantKillDamage = other.GetComponent<CharacterStats>().currentHealth;
            other.GetComponent<CharacterStats>().canBeDamaged = true;
            other.GetComponent<CharacterStats>().TakeDamage(instantKillDamage, 6, true);
        }
    }
}
