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

    public bool teamsActive;

    public GameObject attritionObj;
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
        if (PlayerConfigurationManager.Instance.numOfTeams > 0)
        { teamsActive = true; }
        else
        { teamsActive = false; }
        UIManager.Instance.TeamsActive();
    }

    public void DespawnPlayer(GameObject player)
    {
        player.SetActive(false);
        StartCoroutine(SpawnPlayer(player));
    }
    IEnumerator SpawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(1.5f);
        RespawnPlayer(player);
    }
    public void RespawnPlayer(GameObject player)
    {
        player.GetComponent<CharacterStats>().currentHealth = player.GetComponent<CharacterStats>().startingHealth;
        player.GetComponent<CharacterHandler>().charAnimator.Play("Base Layer.IG_Idle");
        player.GetComponent<CharacterHandler>().charAnimator.Play("Override Layer.New State");
        player.GetComponent<CharacterHandler>().charAnimator.SetBool("AnimBoolDeath", false);
        player.GetComponent<CharacterHandler>().charAnimator.SetBool("AnimBoolStunned", false);
        player.GetComponent<CharacterHandler>().disabled = false;
        player.GetComponent<CharacterHandler>().dashed = false;
        player.GetComponent<CharacterHandler>().playerDead = false;
        player.GetComponent<CharacterHandler>().chargingAbility = false;
        player.GetComponent<CharacterHandler>().shield.SetActive(false);
        player.GetComponent<CharacterHandler>().comboHits = 0;
        player.GetComponent<CharacterHandler>().basicAttackTimer = 0;
        player.GetComponent<CharacterHandler>().weaponHandler.EnableCollider(false);
        player.GetComponent<CharacterHandler>().attacking = false;
        player.GetComponent<CharacterStats>().playerDied = false;
        player.GetComponent<CharacterStats>().canBeDamaged = true;

        player.SetActive(true);
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

    public GameObject endGameEntry;
    public Transform scoreboard;

    public Color[] playerColours;
    public Color[] teamColours;

    public int winningTeam;

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

        GameObject entry = Instantiate(endGameEntry, scoreboard);
        entry.GetComponent<EndScreenEntries>().SpawnEntry("PLAYER " + win, playerColours[winner]);

        if (GameObject.Find("ConfigManager") != null)
        { Destroy(GameObject.Find("ConfigManager")); }
        if (GameObject.Find("GameConfigManager") != null)
        { Destroy(GameObject.Find("GameConfigManager")); }

        menuPlayerObj.SetActive(true);
        menuManagerObj.GetComponent<PlayerInputManager>().enabled = true;
    }
    public void TeamGameOver(bool timeLimit, int teamNum)
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

        string teamColour = "";
        if (teamNum == 1)
        { 
            teamColour = "BLUE TEAM";
            GameObject entry = Instantiate(endGameEntry, scoreboard);
            entry.GetComponent<EndScreenEntries>().SpawnEntry(teamColour, teamColours[teamNum - 1]);
        }
        else if (teamNum == 2)
        { 
            teamColour = "RED TEAM";
            GameObject entry = Instantiate(endGameEntry, scoreboard);
            entry.GetComponent<EndScreenEntries>().SpawnEntry(teamColour, teamColours[teamNum - 1]);
        }
        else if (teamNum == 0)
        { 
            teamColour = "DRAW";
            GameObject entry = Instantiate(endGameEntry, scoreboard);
            entry.GetComponent<EndScreenEntries>().SpawnEntry(teamColour, teamColours[2]);
        }

        if (GameObject.Find("ConfigManager") != null)
        { Destroy(GameObject.Find("ConfigManager")); }
        if (GameObject.Find("GameConfigManager") != null)
        { Destroy(GameObject.Find("GameConfigManager")); }

        menuPlayerObj.SetActive(true);
        menuManagerObj.GetComponent<PlayerInputManager>().enabled = true;
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(1);
    }
}
