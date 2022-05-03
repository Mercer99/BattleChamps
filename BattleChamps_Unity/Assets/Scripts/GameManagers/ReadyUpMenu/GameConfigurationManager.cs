using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigurationManager : MonoBehaviour
{
    public GameMode Gamemode { get; set; }
    public string levelName { get; set; }
    public float gameLength { get; set; }
    public int numOfTeams { get; set; }
    public int team1Size { get; set; }
    public int team2Size { get; set; }

    public static GameConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    public void Destroy()
    {
        Destroy(this);
    }
}
