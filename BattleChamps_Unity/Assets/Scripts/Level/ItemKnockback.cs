using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemKnockback : MonoBehaviour
{
    public Vector3 moveDirection;
    private Rigidbody rb;

    public float playerHitForce = 20f;
    public float playerWalkForce = 1f;
    public float otherForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            moveDirection = (collision.gameObject.transform.position - transform.position).normalized;

            rb.AddForce(moveDirection * playerWalkForce, ForceMode.Impulse);
        }
        else
        {
            moveDirection = (collision.gameObject.transform.position - transform.position).normalized;

            rb.AddForce(moveDirection * otherForce, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            Debug.Log("HIT");
            if ( other.GetComponent<WeaponHandler>().enableDamage )
            {
                moveDirection = (other.gameObject.transform.position - transform.position).normalized;

                rb.AddForce(moveDirection * playerHitForce, ForceMode.Impulse);
            }
        }
    }
}
