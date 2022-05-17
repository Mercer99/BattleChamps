using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public float bombLifetime;

    public Transform forward;
    public ParticleSystem explosion;
    public int playerInt;

    public float bombRadius = 10;
    public float bombDamage = 30;

    bool exploded;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void StartBomb(GameObject parent)
    {
        Physics.IgnoreCollision(parent.transform.root.GetComponent<CharacterController>(), GetComponent<Collider>());
    }

    public IEnumerator BombExplosion()
    {
        rb.AddForce(-forward.forward.normalized * 100, ForceMode.Impulse);
       //rb.AddForce(-forward.up.normalized * 50, ForceMode.Impulse);
        yield return new WaitForSeconds(bombLifetime);

        exploded = true;
        StartCoroutine(BombCollision());
    }
    public IEnumerator BombCollision()
    {
        ParticleSystem particle = Instantiate(explosion, transform.position, transform.rotation);
        particle.Play();
        Destroy(particle, 1f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < bombRadius)
            {
                PlayerConfiguration thisConfig = PlayerConfigurationManager.Instance.playerConfigs[playerInt];
                PlayerConfiguration otherConfig = player.GetComponent<CharacterHandler>().playerConfig;
                if (thisConfig.teamNum <= 0)
                {
                    player.GetComponent<CharacterStats>().TakeDamage(bombDamage, playerInt, false);
                    player.GetComponent<KnockbackReceiver>().Knockback(gameObject.transform.root.gameObject, 25, true, "BOMBED");
                }
                else
                {
                    if (thisConfig.teamNum != otherConfig.teamNum)
                    {
                        player.GetComponent<CharacterStats>().TakeDamage(bombDamage, playerInt, false);
                        player.GetComponent<KnockbackReceiver>().Knockback(gameObject.transform.root.gameObject, 25, true, "BOMBED");
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (exploded == false)
            {
                StopCoroutine(BombExplosion());
                StartCoroutine(BombCollision());
            }
        }
    }
}
