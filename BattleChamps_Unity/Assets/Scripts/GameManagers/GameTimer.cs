using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI pointLimitText;

    public TextMeshProUGUI timerText;
    public float currentTime;
    public bool endless;

    GameObject config;
    public GameConfigurationManager configManager;

    bool timeStarted = false;

    public void Awake()
    {
        if (GameObject.Find("GameConfigManager") != null)
        {
            config = GameObject.Find("GameConfigManager");
            configManager = config.GetComponent<GameConfigurationManager>();
            if (configManager.gameLength > 0)
            { endless = false; currentTime = configManager.gameLength; timerText.text = currentTime.ToString("F0"); }
            else { endless = true; timerText.text = "ENDLESS"; }

            if (configManager.pointLimit > 0)
            { pointLimitText.text = "GOAL: " + configManager.pointLimit; }
            else { pointLimitText.text = "NO GOAL"; }
        }
        else { currentTime = 60; timerText.text = currentTime.ToString("F0"); pointLimitText.text = "GOAL: " + "10"; }
    }

    public void StartTimer()
    {
        if (!endless)
        {
            timeStarted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStarted)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                timerText.text = currentTime.ToString("F0");
            }
            else if (currentTime <= 0)
            {
                if (Mode_AttritionManager.Instance != null)
                {
                    if (PlayerConfigurationManager.Instance.numOfTeams > 0)
                    { GameModeManager.Instance.TeamGameOver(true, Mode_AttritionManager.Instance.winningTeam); }
                    else
                    { GameModeManager.Instance.GameOver(true, Mode_AttritionManager.Instance.leader.playerID); }
                }
                timerText.text = "GAME OVER";
            }
        }
    }
}
