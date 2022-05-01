using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public int playerID;
    public string playerName;

    public float startingHealth = 50f;
    private float maxHealth;
    public float currentHealth;
    public bool playerDied;

    public bool canBeDamaged;
    public Transform particlePos;
    public ParticleSystem hitParticle;
    public AudioClip hitSound;
    public AudioSource audioSource;

    private void OnEnable()
    {
        canBeDamaged = true;
        maxHealth = startingHealth;
        currentHealth = maxHealth;

        GetComponent<PlayerUI_Handler>().EnableUI(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= maxHealth)
        { currentHealth = maxHealth; }

        GetComponent<PlayerUI_Handler>().UpdateUI(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage, string damageDealer)
    {
        if (canBeDamaged)
        {
            currentHealth -= damage;

            ParticleSystem particle = Instantiate(hitParticle, particlePos);
            //GetComponent<ControllerRumbler>().StartRumble(0.5f, playerID);

            if (currentHealth <= 0)
            {
                StartCoroutine(DeathWait());
            }
            else { 
                Camera.main.GetComponent<Screenshake>().StartShake(0);
            }
            audioSource.PlayOneShot(hitSound);
        }
    }
    IEnumerator DeathWait()
    {
        currentHealth = 0; playerDied = true;
        Camera.main.GetComponent<Screenshake>().StartShake(1);
        CameraFollow.Instance.targetPositions.Remove(transform);

        GetComponent<CharacterHandler>().disabled = true;
        GetComponent<CharacterHandler>().StopAllCoroutines();

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void HealHealth()
    {

    }
}
