using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelectionManager : Singleton<SelectionManager>
{
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Components.html

    //Local Multiplayer
    public int numberOfPlayers;

    public GameObject[] Customisers;

    void Start()
    {
    }

    public int NumberOfConnectedDevices()
    {
        return InputSystem.devices.Count;
    }

    public void ReturnToStart()
    {
        SceneManager.LoadScene(0);
    }
}
