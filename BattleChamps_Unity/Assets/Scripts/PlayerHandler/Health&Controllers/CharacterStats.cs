using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int playerID;
    public string playerName;
    public int teamNumber;

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

        GetComponent<PlayerUI_Handler>().teamNum = teamNumber;
        GetComponent<PlayerUI_Handler>().EnableUI(playerID);
        playerName = "Player " + playerID;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= maxHealth)
        { currentHealth = maxHealth; }

        GetComponent<PlayerUI_Handler>().UpdateUI(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage, int damageDealer)
    {
        if (canBeDamaged)
        {
            currentHealth -= damage;

            ParticleSystem particle = Instantiate(hitParticle, particlePos);
            //GetComponent<ControllerRumbler>().StartRumble(0.5f, playerID);

            if (currentHealth <= 0 && playerDied == false)
            {
                playerDied = true;

                UIManager.Instance.PrintOnKillFeed(playerID, damageDealer);

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
        Camera.main.GetComponent<Screenshake>().StartShake(1);

        GetComponent<CharacterHandler>().StopAllCoroutines();

        GetComponent<CharacterHandler>().disabled = true;
        GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(1);
        currentHealth = maxHealth;
        gameObject.transform.position = Mode_AttritionManager.Instance.PlayerSpawns[Random.Range(0, Mode_AttritionManager.Instance.PlayerSpawns.Length - 1)].transform.position;

        GetComponent<CharacterHandler>().disabled = false;
        GetComponent<CharacterController>().enabled = true;
        playerDied = false;

        yield break;
    }
}
