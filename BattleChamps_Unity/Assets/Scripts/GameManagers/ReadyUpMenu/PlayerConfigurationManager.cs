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

    public int numOfTeams;

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
            //Debug.Log("player joined " + pi.playerIndex);
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

    public void SetPlayerValues(int index, int headAccInt, int bodyAccInt, int weaponInt, Ability_Base ability1, Ability_Base ability2)
    {
        int pIndex = index;
        playerConfigs[pIndex].chosenHeadAccessory = headAccInt;
        playerConfigs[pIndex].chosenBodyAccessory = bodyAccInt;
        playerConfigs[pIndex].chosenWeapon = weaponInt;
        playerConfigs[pIndex].chosenAbility1 = ability1;
        playerConfigs[pIndex].chosenAbility2 = ability2;

        ReadyPlayer(pIndex);
    }

    public bool called;

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;

        if (playerConfigs.Count > 1)
        {
            if (playerConfigs.All(p => p.isReady == true))
            {
                if (called == false)
                {
                    if (SceneManager.GetActiveScene().name == "MENU_TeamSelection")
                    {
                        int teamCounterBlue = new int();
                        int teamCounterRed = new int();
                        int neutralTeamCounter = new int();

                        foreach (var playerConfig in playerConfigs)
                        {
                            if (playerConfig.teamNum == 1)
                            { teamCounterBlue++; }
                            if (playerConfig.teamNum == 2)
                            { teamCounterRed++; }
                            if (playerConfig.teamNum <= 0)
                            { neutralTeamCounter++; }
                        }
                        if (teamCounterBlue > 2)
                        { return; }
                        else if (teamCounterRed > 2)
                        { return; }
                        else if ((teamCounterRed + teamCounterBlue > 0) && neutralTeamCounter > 0)
                        { Debug.Log("STOP"); return; }

                        called = true;

                        numOfTeams = teamCounterBlue + teamCounterRed;
                        Debug.Log("TEAMS " + numOfTeams);

                        //SceneManager.LoadScene(6);

                        if (GameObject.Find("GameConfigManager") != null)
                        {
                            SceneManager.LoadScene(GameConfigurationManager.Instance.levelName);
                        }
                        else { SceneManager.LoadScene(scenes[Random.Range(0, scenes.Length - 1)]); }
                    }
                    else
                    { ReadyUpMenuTimer(); }
                }
            }
        }
    }
    public void UnreadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
        if (SceneManager.GetActiveScene().name == "MENU_PlayerSelection")
        {
            UnreadyMenuTimer();
        }
    }

    #region Player Customisation Menu
    public void ShowBackOutMenu(int configNum)
    {
        foreach (GameObject customiser in customisers)
        {
            if (playerConfigs[customiser.GetComponent<CustomiserController>().thisConfig.PlayerIndex].isReady)
            { UnreadyPlayer(customiser.GetComponent<CustomiserController>().thisConfig.PlayerIndex); }

            customiser.GetComponent<CustomiserController>().inputDisabled = true;
        }

        customisers[configNum].GetComponent<CustomiserController>().inputDisabled = false;
    }
    public void Menu_BackToGame()
    {
        foreach (GameObject customiser in customisers)
        {
            customiser.GetComponent<CustomiserController>().inputDisabled = false;
            if (customiser.GetComponent<CustomiserController>().playerReady)
            {
                ReadyPlayer(customiser.GetComponent<CustomiserController>().thisConfig.PlayerIndex);
            }
        }
    }

    public void ReadyUpMenuTimer()
    {
        CustomisationScreenManager.Instance.StartCountdown();
    }
    public void UnreadyMenuTimer()
    {
        CustomisationScreenManager.Instance.StopCountdown();
    }
    #endregion
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

    public Ability_Base chosenAbility1;
    public Ability_Base chosenAbility2;

    public int teamNum { get; set; }
}
