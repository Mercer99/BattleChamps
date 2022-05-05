using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;
using TMPro;

public class GameModeManager : Singleton<GameModeManager>
{
    public CameraFollow cameraFollow;
    public List<GameObject> allPlayers = new List<GameObject>();

    public int pointLimit;

    [HideInInspector]
    public GameObject configManager;

    public GameObject attritionObj;
    public GameObject kingOfTheHillObj;
    public GameObject conquestObj;

    // Start is called before the first frame update
    void Awake()
    {
        allPlayers.Clear();

        if (GameObject.Find("GameConfigManager"))
        {
            configManager = GameObject.Find("GameConfigManager");
            pointLimit = GameConfigurationManager.Instance.pointLimit;

            Instantiate(attritionObj, transform);
        }
        else { pointLimit = 10; Instantiate(attritionObj, transform); }
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

            SpawnCounters(playerConfigs[i]);
        }
    }

    public void SpawnCounters(PlayerConfiguration playerConfig)
    {
        if (Mode_AttritionManager.Instance != null)
        {
            Mode_AttritionManager.Instance.SpawnObjs(playerConfig);
        }
        //else if (Mode_KingOfTheHill.Instance != null) { }
        //else if (Mode_Conquest.Instance != null) { } 
    }

    public GameObject timeImage;
    public GameObject pointImage;

    public GameObject menuPlayerObj;
    public GameObject menuManagerObj;

    public GameObject endScreen;

    public TextMeshProUGUI winnerName;

    public bool gameEnded = false;

    public void GameOver(bool timeLimit, int winner)
    {
        if (gameEnded)
        { return; }

        gameEnded = true;
        GetComponent<GameTimer>().currentTime = 0;
        endScreen.SetActive(true);
        endScreen.GetComponent<Animator>().Play("EndScreenPopup");

        if (timeLimit)
        {
            timeImage.SetActive(true);
        }
        else
        {
            pointImage.SetActive(true);
        }

        string win = (winner + 1).ToString();
        winnerName.text = "PLAYER " + win;
        
        if (GameObject.Find("ConfigManager") != null)
        { Destroy(GameObject.Find("ConfigManager")); }
        if (GameObject.Find("GameConfigManager") != null)
        { Destroy(GameObject.Find("GameConfigManager")); }

        menuPlayerObj.SetActive(true);
        menuManagerObj.GetComponent<PlayerInputManager>().enabled = true;
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}
