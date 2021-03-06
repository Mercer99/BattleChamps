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

    public ParticleSystem hitParticle;
    public Transform particlePos;

    public bool canBeDamaged;
    public AudioClip hitSound;
    public AudioSource audioSource;

    public int lastCharacter;
    public float hitCD = 5;
    private float lastHitCD;

    private float hitCooldown;
    public float lastHitCooldown = 1;

    private void OnEnable()
    {
        
    }
    public void InitializePlayerStats()
    {
        canBeDamaged = true;
        maxHealth = startingHealth;
        currentHealth = maxHealth;

        GetComponent<PlayerUI_Handler>().EnableUI(playerID);
        playerName = "Player " + playerID;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= maxHealth)
        { currentHealth = maxHealth; }

        GetComponent<PlayerUI_Handler>().UpdateUI(currentHealth, maxHealth);

        if (lastHitCD > 0)
        { lastHitCD -= Time.deltaTime; }
        else { lastHitCD = 0; }
    }

    private void FixedUpdate()
    {
        if (hitCooldown > 0)
        { hitCooldown -= Time.deltaTime; }
        else { hitCooldown = 0; }
    }

    public void TakeDamage(float damage, int damageDealer, bool goThroughBlock)
    {
        if (canBeDamaged || goThroughBlock)
        {
            if (damageDealer != lastCharacter)
            { hitCooldown = lastHitCooldown; }

            if ((damageDealer == lastCharacter && hitCooldown <= 0) || damageDealer != lastCharacter)
            {
                currentHealth -= damage;

                GetComponent<ControllerRumbler>().StartRumble(0.5f, playerID);

                ParticleSystem hitEffect = Instantiate(hitParticle, particlePos);
                hitEffect.Play();

                if (damageDealer < 4)
                {
                    lastCharacter = damageDealer;
                    lastHitCD = hitCD;
                }

                if (currentHealth <= 0 && playerDied == false)
                {
                    playerDied = true;

                    if (lastHitCD > 0)
                    { UIManager.Instance.PrintOnKillFeed(playerID, lastCharacter); }
                    else { UIManager.Instance.PrintOnKillFeed(playerID, damageDealer); }
                    Debug.Log(lastCharacter);
                    StartCoroutine(DeathWait());
                }
                else
                { Camera.main.GetComponent<Screenshake>().StartShake(0); }

                audioSource.PlayOneShot(hitSound);
            }
        }
    }
    IEnumerator DeathWait()
    {
        Camera.main.GetComponent<Screenshake>().StartShake(1);

        GetComponent<CharacterHandler>().StopAllCoroutines();
        GetComponent<CharacterHandler>().charAnimator.SetBool("AnimBoolDeath", true);
        GetComponent<CharacterHandler>().disabled = true;
        GetComponent<CharacterHandler>().playerDead = true;
        GetComponent<CharacterHandler>().DeactivateWhirlwind();

        yield return new WaitForSeconds(0.5f);

        Transform spawn = GameModeManager.Instance.PlayerSpawns[Random.Range(0, GameModeManager.Instance.PlayerSpawns.Length - 1)].transform;
        gameObject.transform.position = new Vector3(spawn.position.x, spawn.position.y, spawn.position.z);
        GameModeManager.Instance.DespawnPlayer(gameObject);

        yield break;
    }
}
