using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_Handler : MonoBehaviour
{
    //public Color playerColour;
    public GameObject[] playerNumber;

    public Color[] playerColours;
    [HideInInspector]
    public Color playerColour;

    public Image healthBar;
    public Image dashIcon;
    public Image shieldIcon;
    public Image ability1Icon;
    public Image ability2Icon;

    private float maxDashCD = 1, dashCD = 0;
    private float maxShieldCD = 1, shieldCD = 0;
    private float maxAbility1CD = 1, ability1CD = 0;
    private float maxAbility2CD = 1, ability2CD = 0;

    public Image[] playerUI;

    // Start is called before the first frame update
    public void EnableUI(int playerID)
    {
        foreach (Image image in playerUI)
        {
            image.color = playerColours[playerID];
            playerColour = playerColours[playerID];
        }

        foreach (GameObject player in playerNumber)
        { player.SetActive(false); }

        playerNumber[playerID].SetActive(true);
    }

    // Called through character stats
    public void UpdateUI(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;

        DashCD();
        ShieldCD();
        Ability1CD();
        Ability2CD();
    }

    #region Ability Cooldowns

    #region Dash CD
    public void DashCDTimer(float cooldown)
    {
        dashCD = cooldown;
        maxDashCD = cooldown;
    }
    void DashCD()
    {
        if (dashCD > 0)
        { dashCD -= Time.deltaTime; }
        dashIcon.fillAmount = dashCD / maxDashCD;
    }
    #endregion
    #region Shield CD
    public void ShieldCDTimer(float cooldown)
    {
        shieldCD = cooldown;
        maxShieldCD = cooldown;
    }
    void ShieldCD()
    {
        if (shieldCD > 0)
        { shieldCD -= Time.deltaTime; }
        shieldIcon.fillAmount = shieldCD / maxShieldCD;
    }
    #endregion
    #region Ability 1 CD
    public void Ability1CDTimer(float cooldown)
    {
        ability1CD = cooldown;
        maxAbility1CD = cooldown;
    }
    void Ability1CD()
    {
        if (ability1CD > 0)
        { ability1CD -= Time.deltaTime; }
        ability1Icon.fillAmount = ability1CD / maxAbility1CD;
    }
    #endregion
    #region Ability 2 CD
    public void Ability2CDTimer(float cooldown)
    {
        ability2CD = cooldown;
        maxAbility2CD = cooldown;
    }
    void Ability2CD()
    {
        if (ability2CD > 0)
        { ability2CD -= Time.deltaTime; }
        ability2Icon.fillAmount = ability2CD / maxAbility2CD;
    }
    #endregion

    #endregion
}
