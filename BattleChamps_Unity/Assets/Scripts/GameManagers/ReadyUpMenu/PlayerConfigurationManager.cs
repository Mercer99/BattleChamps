using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    [SerializeField]
    private int MaxPlayers = 4;

    public GameObject[] customisers;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }

    }
    public void Destroy()
    {
        Destroy(this);
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("player joined " + pi.playerIndex);
        pi.transform.SetParent(transform);

        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
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
        playerConfigs[index].chosenHeadAccessory = headAccInt;
        playerConfigs[index].chosenBodyAccessory = bodyAccInt;
        playerConfigs[index].chosenWeapon = weaponInt;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
        if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            SceneManager.LoadScene(2);
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
}
