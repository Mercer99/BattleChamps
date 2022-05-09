using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnShieldRipples : MonoBehaviour
{
    public void OnEnable()
    {
        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), GetComponent<Collider>());
    }

    public GameObject shieldRipple;

    private VisualEffect shieldRipplesVFX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Environment")
        {
            var Ripples = Instantiate(shieldRipple, transform) as GameObject;
            shieldRipplesVFX = Ripples.GetComponent<VisualEffect>();
            shieldRipplesVFX.SetVector3("SphereCenter", collision.contacts[0].point);

            Destroy(Ripples, 2);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Environment")
        {
            var Ripples = Instantiate(shieldRipple, transform) as GameObject;
            shieldRipplesVFX = Ripples.GetComponent<VisualEffect>();
            shieldRipplesVFX.SetVector3("SphereCenter", other.ClosestPoint(other.transform.position));

            Destroy(Ripples, 2);
        }
    }
}
