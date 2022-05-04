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

    public int playerInt;

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
        GetComponent<Animator>().Play("Explode");
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}
