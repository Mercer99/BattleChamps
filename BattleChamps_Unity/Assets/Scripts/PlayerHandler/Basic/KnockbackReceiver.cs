using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    public float knockbackAmount = 50f;
    public float knockbackTime = 0.5f;
    private float currentKnockbacktime = 0;

    Vector3 knockbackDirection;

    public void Knockback(GameObject player)
    {
        knockbackDirection = (player.transform.position - transform.position).normalized;
        Debug.Log(knockbackDirection);
        currentKnockbacktime = knockbackTime;
    }

    void Update()
    {
        if (currentKnockbacktime > 0)
        {
            GetComponent<CharacterHandler>().disabled = true;
            GetComponent<CharacterController>().Move(-knockbackDirection * knockbackAmount * Time.deltaTime);
            currentKnockbacktime -= Time.deltaTime;
        }
        else { GetComponent<CharacterHandler>().disabled = false; }
    }
}
