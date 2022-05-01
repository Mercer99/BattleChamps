using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private Collider weaponCollider;

    public float weaponDamage;
    [HideInInspector]
    public bool enableDamage = false;

    private GameObject playerObj;
    private string playerName;

    void Start()
    {
        weaponCollider = GetComponent<Collider>();
        enableDamage = false;

        playerObj = transform.root.gameObject;
        playerName = "DAVE"; //playerObj.GetComponent<CharacterStats>().playerName;
        
        // Ignore collision between weapon & weapon holder
        Physics.IgnoreCollision(playerObj.GetComponent<CharacterController>(), weaponCollider);
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
