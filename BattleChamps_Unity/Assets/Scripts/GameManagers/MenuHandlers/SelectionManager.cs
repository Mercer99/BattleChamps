using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    SinglePlayer,
    LocalMultiplayer
}

public class SelectionManager : Singleton<SelectionManager>
{
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Components.html
    //Game Mode
    public GameMode currentGameMode;

    public GameObject[] inScenePlayers;

    //Local Multiplayer
    public int numberOfPlayers;

    //Spawned Players
    private List<PlayerController> activePlayerControllers;
    private PlayerController focusedPlayerController;

    void Start()
    {
        SetupBasedOnGameState();
    }

    void SetupBasedOnGameState()
    {
        switch (currentGameMode)
        {
            case GameMode.SinglePlayer:
                SetupSinglePlayer();
                break;

            case GameMode.LocalMultiplayer:
                SetupLocalMultiplayer();
                break;
        }
    }

    void SetupSinglePlayer()
    {
        activePlayerControllers = new List<PlayerController>();

        if (inScenePlayers[0] == true)
        {
            AddPlayerToActivePlayerList(inScenePlayers[0].GetComponent<PlayerController>());
        }

        SetupActivePlayers();
    }

    void SetupLocalMultiplayer()
    {
        ActivatePlayers();

        SetupActivePlayers();
    }

    void ActivatePlayers()
    {
        activePlayerControllers = new List<PlayerController>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            //GameObject spawnedPlayer = ;
            //AddPlayerToActivePlayerList(spawnedPlayer.GetComponent<PlayerController>());
        }
    }

    void AddPlayerToActivePlayerList(PlayerController newPlayer)
    {
        activePlayerControllers.Add(newPlayer);
    }

    void SetupActivePlayers()
    {
        for (int i = 0; i < activePlayerControllers.Count; i++)
        {
            activePlayerControllers[i].SetupPlayer(i);
        }
    }

    //Get Data ----

    public List<PlayerController> GetActivePlayerControllers()
    {
        return activePlayerControllers;
    }

    public PlayerController GetFocusedPlayerController()
    {
        return focusedPlayerController;
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }
}
