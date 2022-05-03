using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode_AttritionManager : Singleton<Mode_AttritionManager>
{
    public CameraFollow cameraFollow;
    public List<GameObject> allPlayers = new List<GameObject>();

    public GameObject configManager;

    // Start is called before the first frame update
    void Awake()
    {
        configManager = GameObject.Find("ConfigManager");
        allPlayers.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerJoined(GameObject player)
    {
        allPlayers.Add(player);
        cameraFollow.targetPositions.Add(player.transform);
    }

    [SerializeField]
    private Transform[] PlayerSpawns;

    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation);
            playerConfigs[i].Input.SwitchCurrentActionMap("InGameActions");
            player.GetComponent<CharacterHandler>().InitializePlayer(playerConfigs[i]);
        }
    }
}
