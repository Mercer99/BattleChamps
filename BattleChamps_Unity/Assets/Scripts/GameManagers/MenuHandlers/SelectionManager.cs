using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : Singleton<SelectionManager>
{
    public GameObject[] Arenas;

    public GameObject teamMenu;

    public GameObject configManager;

    public void Awake()
    {
        configManager = GameObject.Find("ConfigManager");

        if (GameObject.Find("GameConfigManager") != null)
        {
            GameObject gameConfig;

            gameConfig = GameObject.Find("GameConfigManager");

            foreach (GameObject arena in Arenas)
            {
                if (gameConfig.GetComponent<GameConfigurationManager>().levelName == arena.name)
                { arena.SetActive(true); break; }
            }
        }
        else { Arenas[0].SetActive(true); }
    }

    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

            for (int i = 0; i < playerConfigs.Length; i++)
            {
                var selectionMenu = Instantiate(playerPrefab, teamMenu.transform);
                playerConfigs[i].Input.SwitchCurrentActionMap("MenuActions");
                playerConfigs[i].isReady = false;
                selectionMenu.GetComponent<TeamSelectionMenu>().InitializePlayer(playerConfigs[i]);
            }
        }
    }
}
