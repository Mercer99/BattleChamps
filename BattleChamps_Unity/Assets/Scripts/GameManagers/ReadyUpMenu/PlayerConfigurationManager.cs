using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    public List<PlayerConfiguration> playerConfigs;
    [SerializeField]
    private int maxPlayers = 4;

    public GameObject[] customisers;

    public int levelNum;

    public string[] scenes;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a second instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }
    public void Destroy()
    {
        Destroy(this);
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (playerConfigs.Count < maxPlayers)
        {
            Debug.Log("player joined " + pi.playerIndex);
            pi.transform.SetParent(transform);

            if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
            {
                playerConfigs.Add(new PlayerConfiguration(pi));
            }
        }
    }
    public void HandlePlayerLeft(PlayerInput pi)
    {
        foreach (PlayerConfiguration config in playerConfigs)
        {
            if (config == null)
            {
                playerConfigs.Remove(config);
            }
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerValues(int index, int headAccInt, int bodyAccInt, int weaponInt)
    {
        int pIndex = index;
        playerConfigs[pIndex].chosenHeadAccessory = headAccInt;
        playerConfigs[pIndex].chosenBodyAccessory = bodyAccInt;
        playerConfigs[pIndex].chosenWeapon = weaponInt;

        ReadyPlayer(pIndex);
    }

    public bool called;

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;

        if (playerConfigs.All(p => p.isReady == true))
        {
            if (called == false)
            {
                if (SceneManager.GetActiveScene().name == "MENU_TeamSelection")
                {
                    Debug.Log("ALL READY");
                    called = true;

                    SceneManager.LoadScene(5);

                    if (GameObject.Find("GameConfigManager") != null)
                    {
                    SceneManager.LoadScene(GameConfigurationManager.Instance.levelName);
                    }
                    else { SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length - 1)]); }
                }
                else { SceneManager.LoadScene("MENU_TeamSelection"); }
            }
        }
    }
    public void UnreadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; private set; }
    public int PlayerIndex { get; private set; }
    public bool isReady { get; set; }
    public int chosenWeapon { get; set; }
    public int chosenHeadAccessory { get; set; }
    public int chosenBodyAccessory { get; set; }

    public int teamNum { get; set; }
}
