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
    private int playerName;

    public VisualEffect hitEffect;

    void OnEnable()
    {
        weaponCollider = GetComponent<Collider>();
        enableDamage = false;

        playerObj = transform.root.gameObject;
        playerName = playerObj.GetComponent<CharacterStats>().playerID;
        
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
                other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerName, false);
                other.GetComponent<KnockbackReceiver>().Knockback(playerObj);

                hitEffect.Play();
                //enableDamage = false;
            }
        }
    }
}
