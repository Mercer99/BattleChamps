using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponHandler : MonoBehaviour
{
    private Collider weaponCollider;

    public float weaponDamage;
    [HideInInspector]
    public bool enableDamage = false;

    private GameObject playerObj;

    public VisualEffect hitEffect;

    void OnEnable()
    {
        weaponCollider = GetComponent<Collider>();
        enableDamage = false;

        playerObj = transform.root.gameObject;
        
        // Ignore collision between weapon & weapon holder
        Physics.IgnoreCollision(playerObj.GetComponent<CharacterController>(), weaponCollider);
    }

    public void EnableCollider(bool colliderEnable)
    { enableDamage = colliderEnable; }

    private void OnTriggerEnter(Collider other)
    {
        if (enableDamage)
        {
            if (other.gameObject.tag == "Player")
            {
                if (other.GetComponent<CharacterHandler>().shield.activeInHierarchy)
                { playerObj.GetComponent<CharacterHandler>().StunPlayer(2); playerObj.GetComponent<CharacterHandler>().SpawnNotification("PARRIED"); }

                other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerObj.GetComponent<CharacterStats>().playerID, false);
                other.GetComponent<KnockbackReceiver>().Knockback(playerObj, 25, false, "");

                hitEffect.Play();
                //enableDamage = false;
            }
        }
    }
}
