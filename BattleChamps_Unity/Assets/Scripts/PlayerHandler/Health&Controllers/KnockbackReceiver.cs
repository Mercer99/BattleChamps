using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    public float knockbackAmount = 25f;
    public float knockbackTime = 0.5f;
    private float currentKnockbacktime = 0;
    private bool knocked;

    private Transform knocker;
    Vector3 knockbackDirection;

    public void Knockback(GameObject player, float knockbackValue, bool spawnNotif, string notifText)
    {
        knocker = player.transform;
        knockbackAmount = knockbackValue;
        knocked = true;
        knockbackDirection = (player.transform.position - transform.position).normalized;
        Debug.Log(knockbackDirection);
        currentKnockbacktime = knockbackTime;

        if (spawnNotif)
        { GetComponent<CharacterHandler>().SpawnNotification(notifText); }
    }

    void Update()
    {
        if (currentKnockbacktime > 0)
        {
            GetComponent<CharacterController>().Move(-knockbackDirection * knockbackAmount * Time.deltaTime);
            GetComponent<CharacterHandler>().disabled = true;
            transform.LookAt(-(new Vector3(knocker.position.x, transform.position.y, knocker.position.z)));
            currentKnockbacktime -= Time.deltaTime;
        }
        else if (currentKnockbacktime <= 0 && knocked == true)
        { GetComponent<CharacterHandler>().disabled = false; knocked = false; }
    }
}
