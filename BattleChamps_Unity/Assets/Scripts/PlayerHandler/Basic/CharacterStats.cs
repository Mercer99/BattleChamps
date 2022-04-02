using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public int playerID;
    public string playerName;

    public float startingHealth = 50f;
    private float maxHealth;
    private float currentHealth;
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

            if (currentHealth <= 0)
            {
                currentHealth = 0; playerDied = true;
                Camera.main.GetComponent<Screenshake>().StartShake(1);
            }
            else { 
                Camera.main.GetComponent<Screenshake>().StartShake(0);
            }
            audioSource.PlayOneShot(hitSound);

            ParticleSystem particle = Instantiate(hitParticle, particlePos);
        }
    }

    public void HealHealth()
    {

    }
}
