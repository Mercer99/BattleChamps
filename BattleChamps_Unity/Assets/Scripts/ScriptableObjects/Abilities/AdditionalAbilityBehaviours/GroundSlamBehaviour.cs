using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamBehaviour : MonoBehaviour
{
    private float damage;
    private GameObject parentObj;
    private GameObject playerObj;

    public ParticleSystem slamParticle;

    public void Start()
    {
        Destroy(gameObject, 1);
    }

    public void Damage(GameObject parent, float slamRange, float slamDamage)
    {
        damage = slamDamage;
        parentObj = parent;

        parent.GetComponent<CharacterHandler>().chargingAbility = true;
        Invoke("StopAbility", 0.6f);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Invoke("Particle", 0.3f);

        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, parent.transform.position) < slamRange)
            {
                if (player != parent)
                {
                    playerObj = player;
                    Invoke("DoDamage", 0.3f);
                }
            }
        }
    }
    private void DoDamage()
    {
        playerObj.GetComponent<CharacterStats>().TakeDamage(damage, parentObj.GetComponent<CharacterHandler>().playerConfig.PlayerIndex, false);
        playerObj.GetComponent<KnockbackReceiver>().Knockback(parentObj.transform.root.gameObject);
    }

    private void Particle()
    {
        ParticleSystem particle = Instantiate(slamParticle, parentObj.transform);
        particle.Play();
        Destroy(particle, 1f);
    }
    private void StopAbility()
    {
        parentObj.GetComponent<CharacterHandler>().chargingAbility = false;
        Destroy(gameObject, 1);
    }
}
