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

            playerMenu.InitializePlayer(PlayerConfigurationManager.Instance.playerConfigs[PlayerConfigurationManager.Instance.playerConfigs.Count-1]);
        }
    }
}
