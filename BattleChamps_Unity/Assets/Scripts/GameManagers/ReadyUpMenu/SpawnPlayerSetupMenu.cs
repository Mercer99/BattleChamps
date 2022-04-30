using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    private GameObject rootMenu;
    public PlayerInput input;

    private bool playerReady;

    private CustomiserController playerMenu;

    private void Awake()
    {
        rootMenu = GameObject.Find("Customisers");
        if (rootMenu != null)
        {
            GameObject menu = GameObject.Find("ConfigManager").GetComponent<PlayerConfigurationManager>().customisers[input.playerIndex];
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().setPlayerIndex(input.playerIndex);
            menu.GetComponent<CustomiserController>().PlayerJoined();
            playerMenu = menu.GetComponent<CustomiserController>();
        }
    }

    public void BackOut(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerReady)
            {
                playerMenu.PlayerUnready();
                playerReady = false;
                return;
            }
            else { PlayerConfigurationManager.Instance.Destroy(); 
                SceneManager.LoadScene(0); }
        }   
    }
    public void NavigateScroll(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (context.action.WasPressedThisFrame())
        {
            if (playerMenu.inputDisabled)
            { return; }
            if (scrollValue.y > 0)
            {
                playerMenu.onPressUp();
            }
            else if (scrollValue.y < 0)
            {
                playerMenu.onPressDown();
            }
        }

        if (context.action.WasReleasedThisFrame())
        {
            playerMenu.onReleaseUp();
            playerMenu.onReleaseDown();
            if (scrollValue.y > 0)
            {
                playerMenu.onReleaseUp();
            }
            else if (scrollValue.y < 0)
            {
                playerMenu.onReleaseDown();
            }
        }
    }
    public void ChangeMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        { playerMenu.MenuSwap(); }
    }
    public void NavigateChangeItemLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerMenu.PreviousItem();
        }
    }
    public void NavigateChangeItemRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerMenu.NextItem();
        }
    }
    public void Ready(InputAction.CallbackContext context)
    {
        playerMenu.PlayerReady();
        playerReady = true;
    }
}
