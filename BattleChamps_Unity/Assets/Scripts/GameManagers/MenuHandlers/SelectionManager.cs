using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelectionManager : Singleton<SelectionManager>
{
    public GameObject[] Arenas;

    public GameObject teamMenu;

    public GameObject gameConfig;
    public GameObject configManager;

    public void Awake()
    {
        configManager = GameObject.Find("ConfigManager");

        if (GameObject.Find("GameConfigManager") != null)
        {
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
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var selectionMenu = Instantiate(playerPrefab, teamMenu.transform);
            playerConfigs[i].Input.SwitchCurrentActionMap("MenuActions");
            selectionMenu.GetComponent<TeamSelectionMenu>().InitializePlayer(playerConfigs[i]);
        }
    }
}
