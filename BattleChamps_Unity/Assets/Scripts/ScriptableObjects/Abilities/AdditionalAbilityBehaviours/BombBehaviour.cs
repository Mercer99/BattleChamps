using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public float bombLifetime;

    public Transform forward;

    public ParticleSystem explosion;
    public GameObject particleSpawnpoint;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public IEnumerator BombExplosion()
    {
        rb.AddForce(-forward.forward.normalized * 50, ForceMode.Impulse);
        yield return new WaitForSeconds(bombLifetime);
        ParticleSystem particle = Instantiate(explosion, particleSpawnpoint.transform);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
