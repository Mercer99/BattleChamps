using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode_AttritionManager : Singleton<Mode_AttritionManager>
{
    public int pointLimit;

    public CameraFollow cameraFollow;
    public List<GameObject> allPlayers = new List<GameObject>();

    public GameObject configManager;

    public List<KillCounter> killCounts;

    public GameObject killCounterObj;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.Find("GameConfigManager"))
        {
            configManager = GameObject.Find("GameConfigManager");
            pointLimit = GameConfigurationManager.Instance.pointLimit;
        }
        else { pointLimit = 1; }

        allPlayers.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckKills(int playerID)
    {
        if (killCounts[playerID].counter >= pointLimit)
        { Debug.Log("GAMEOVER"); } //GameOver
    }

    public void AddKill(int playerID)
    {
        killCounts[playerID].counter++;

        CheckKills(playerID);
    }

    public void PlayerJoined(GameObject player)
    {
        allPlayers.Add(player);
        cameraFollow.targetPositions.Add(player.transform);
    }

    [SerializeField]
    public Transform[] PlayerSpawns;

    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    public void SpawnPlayers()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation);
            playerConfigs[i].Input.SwitchCurrentActionMap("InGameActions");
            player.GetComponent<CharacterHandler>().InitializePlayer(playerConfigs[i]);

            var killCounter = Instantiate(killCounterObj, transform);
            killCounts.Add(killCounter.GetComponent<KillCounter>());
            killCounter.GetComponent<KillCounter>().playerID = playerConfigs[i].PlayerIndex;
        }
    }
}
