using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public string playerName;

    public float startingHealth = 50f;
    private float maxHealth;
    private float currentHealth;
    public bool playerDied;

    public Image healthBar;
    public GameObject dashCDObject;
    public GameObject dashCDPObject;
    private float maxDashCD;
    private float dashCD;

    public Image playerIdentifier;
    public Color playerColour;

    public bool canBeDamaged;

    private void OnEnable()
    {
        playerColour = new Color(Random.value, Random.value, Random.value, 255);
        playerIdentifier.color = playerColour;
        dashCDPObject.SetActive(false);

        canBeDamaged = true;
        maxHealth = startingHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = currentHealth/ maxHealth;

        if (currentHealth >= maxHealth)
        { currentHealth = maxHealth; }

        dashCDPObject.SetActive(dashCD > 0);
        if (dashCD > 0)
        { dashCD -= Time.deltaTime; }
        dashCDObject.GetComponent<Image>().fillAmount = dashCD / maxDashCD;
    }

    public void DashCDTimer(float cooldown)
    {
       dashCD = cooldown;
        maxDashCD = cooldown;
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
            else { Camera.main.GetComponent<Screenshake>().StartShake(0); }
        }
    }

    public void HealHealth()
    {

    }
}
