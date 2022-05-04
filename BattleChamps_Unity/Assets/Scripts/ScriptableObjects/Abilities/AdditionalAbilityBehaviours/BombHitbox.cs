using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHitbox : MonoBehaviour
{
    public float bombDamage;
    public BombBehaviour bomb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<CharacterStats>().TakeDamage(bombDamage, bomb.playerInt);
            other.GetComponent<KnockbackReceiver>().Knockback(gameObject.transform.root.gameObject);
        }
    }
}
