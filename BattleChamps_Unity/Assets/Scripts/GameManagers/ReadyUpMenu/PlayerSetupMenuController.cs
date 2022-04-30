using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;
    public void setPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SelectItems()
    {
        if (!inputEnabled) { return; }
        CustomiserController controller = GetComponent<CustomiserController>();
        PlayerConfigurationManager.Instance.SetPlayerValues(playerIndex, controller.currentHeadAccessory, 
            controller.currentBodyAccessory, controller.currentWeapon);
        ReadyPlayer();
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
    }
    public void UnreadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.UnreadyPlayer(playerIndex);
    }
}