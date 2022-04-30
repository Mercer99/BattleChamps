using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReactionParticle : MonoBehaviour
{
    public Material[] particleMaterials;

    public float time;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<ParticleSystemRenderer>().material = particleMaterials[Random.Range(0, particleMaterials.Length)];
        Destroy(gameObject, GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        GetComponent<ParticleSystem>().Play();
    }
}
