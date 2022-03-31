using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private MeshCollider weaponCollider;

    public float weaponDamage;
    [HideInInspector]
    public bool enableDamage = false;

    private GameObject playerObj;
    private string playerName;


    void OnEnable()
    {
        weaponCollider = GetComponent<MeshCollider>();
        enableDamage = false;

        playerObj = transform.root.gameObject;
        playerName = playerObj.GetComponent<CharacterStats>().playerName;
        
        // Ignore collision between weapon & weapon holder
        Physics.IgnoreCollision(weaponCollider, playerObj.GetComponent<CharacterController>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCollider(bool colliderEnable)
    { enableDamage = colliderEnable; }

    private void OnTriggerEnter(Collider other)
    {
        if (enableDamage)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerName);
                other.GetComponent<KnockbackReceiver>().Knockback(playerObj);
                enableDamage = false;
            }
        }
    }
}
