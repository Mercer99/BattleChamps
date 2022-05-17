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
            PlayerConfiguration thisConfig = PlayerConfigurationManager.Instance.playerConfigs[playerName];
            PlayerConfiguration otherConfig = other.GetComponent<CharacterHandler>().playerConfig;
            if (thisConfig.teamNum <= 0)
            {
                other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerName, false);
                other.GetComponent<KnockbackReceiver>().Knockback(playerObj, 10, false, "");
            }
            else
            {
                if (thisConfig.teamNum != otherConfig.teamNum)
                {
                    other.GetComponent<CharacterStats>().TakeDamage(weaponDamage, playerName, false);
                    other.GetComponent<KnockbackReceiver>().Knockback(playerObj, 10, false, "");
                }
            }
        }
    }
}
