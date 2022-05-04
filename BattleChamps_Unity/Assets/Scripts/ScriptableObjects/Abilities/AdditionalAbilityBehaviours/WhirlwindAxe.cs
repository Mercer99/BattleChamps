using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindAxe : MonoBehaviour
{
    private Collider weaponCollider;

    public float weaponDamage;

    private GameObject playerObj;
    private int playerName;

    void Start()
    {
        weaponCollider = GetComponent<Collider>();

        playerObj = transform.root.gameObject;
        playerName = playerObj.GetComponent<CharacterStats>().playerID;

        // Ignore collision between weapon & weapon holder
        Physics.IgnoreCollision(playerObj.GetComponent<CharacterController>(), weaponCollider);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerName);
            other.GetComponent<KnockbackReceiver>().Knockback(playerObj);
        }
    }
}
