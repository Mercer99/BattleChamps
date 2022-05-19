using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public bool gamePaused = false;
    public GameObject pauseMenuPanel;

    public List<GameObject> playerList = new List<GameObject>();

    public GameObject coloursPanel;
    public GameObject coloursPrefab;

    public GameObject killFeedEntry;
    public GameObject killFeed;

    public string[] killVerses;

    public TextMeshProUGUI[] playerUI;
    public Image[] playerTeamIndicator;
    public Color[] teamColors;

    bool initiated;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenuPanel.SetActive(false);
        Pause_ResumeGame();
        initiated = false;
    }

    public void TeamsActive()
    {
        if (initiated)
        { return; }

        if (PlayerConfigurationManager.Instance.numOfTeams > 0)
        {
            Debug.Log("SPAWNED");
            foreach (var config in PlayerConfigurationManager.Instance.playerConfigs)
            {
                playerTeamIndicator[config.PlayerIndex].color = teamColors[config.teamNum -1];
            }
        }
        else
        {
            Debug.Log("FALSE");
            foreach (Image indicator in playerTeamIndicator)
            {
                indicator.gameObject.SetActive(false);
            }
        }
        initiated = true;
    }

    public void PrintOnKillFeed(int deadPlayer, int killer)
    {
        if (killer > PlayerConfigurationManager.Instance.playerConfigs.Count - 1)
        { return; }

        if (GameConfigurationManager.Instance != null)
        {
            if (GameConfigurationManager.Instance.currentGamemode.gamemodeName == "Attrition")
            {
                if(deadPlayer == killer)
                { return; }

                Mode_AttritionManager.Instance.AddKill(killer);
            }
            else if (GameConfigurationManager.Instance.currentGamemode.gamemodeName == "Conquest")
            {
                if (deadPlayer == killer)
                { return; }

                //Mode_ConquestManager.Instance.AddPoints(killer);
            }
        }
        else 
        {
            if (deadPlayer == killer)
            { return; }
            Mode_AttritionManager.Instance.AddKill(killer);
        }

        string verse = killVerses[Random.Range(0, killVerses.Length - 1)];
        int killerNum = killer + 1;
        int killedPlayer = deadPlayer + 1;
        string entry = "Player " + killerNum + " " + verse + " Player " + killedPlayer;
        GameObject killEntry = Instantiate(killFeedEntry, killFeed.transform);
        killEntry.GetComponent<KillFeedEntry>().killFeedText.text = entry;
    }

    #region Pause Menu Functions
    public void Pause_ResumeGame()
    {
        gamePaused = false;
        Pause_PausePanelActive();
    }
    public void Pause_RestartGame()
    {
        SceneManager.LoadScene(1);
        Pause_ResumeGame();
    }
    public void Pause_QuitGame()
    {
        Application.Quit();
    }

    public void Pause_TogglePause()
    {
        gamePaused = !gamePaused;
        Pause_PausePanelActive();
    }
    // Set Pause Menu active equal to game paused bool
    private void Pause_PausePanelActive()
    {
        pauseMenuPanel.SetActive(gamePaused ? true : false);
        Time.timeScale = gamePaused ? 0 : 1;
    }
    #endregion
}
