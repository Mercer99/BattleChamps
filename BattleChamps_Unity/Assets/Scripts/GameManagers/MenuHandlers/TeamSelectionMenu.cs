using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class TeamSelectionMenu : MonoBehaviour
{
    public Color[] playerColours;

    public Color readyColour;
    public Color unreadyColour;

    private PlayerControls controls;
    public bool inputDisabled;

    public GameObject[] playerIcon;
    public Image playerRing;
    public Image controllerIcon;

    public Sprite xbControllerSprite;
    public Sprite psControllerSprite;

    public int playerIndex;

    bool ready;

    public PlayerConfiguration playerConfig;

    public void Awake()
    { controls = new PlayerControls(); }

    private void Update()
    {
        playerRing.color = playerConfig.isReady ? readyColour : unreadyColour;
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        playerIndex = config.PlayerIndex;

        playerIcon[config.PlayerIndex].SetActive(true);
        controllerIcon.color = playerColours[config.PlayerIndex];

        config.Input.uiInputModule = GetComponentInChildren<InputSystemUIInputModule>();

        config.Input.onActionTriggered += Input_onActionTriggered;

        if (config.Input.devices[0].device.name == "XInputControllerWindows")
        { controllerIcon.sprite = xbControllerSprite; }
        else if (config.Input.devices[0].device.name == "DualShock4GamepadHID")
        { controllerIcon.sprite = psControllerSprite; }
        else { controllerIcon.sprite = xbControllerSprite; }
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.MenuActions.Submit.name)
        {
            if (!ready)
            { ready = true; PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex); }
        }
        if (obj.action.name == controls.MenuActions.Cancel.name)
        {
            if (obj.performed)
            {
                if (ready)
                {
                    ready = false;
                    PlayerConfigurationManager.Instance.UnreadyPlayer(playerIndex);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
        if (obj.action.name == controls.MenuActions.ShuffleLeft.name)
        {
            if (obj.performed)
            { }//HERE }
        }
        if (obj.action.name == controls.MenuActions.ShuffleRight.name)
        {
            if (obj.performed)
            { }//HERE }
        }
    }
}
