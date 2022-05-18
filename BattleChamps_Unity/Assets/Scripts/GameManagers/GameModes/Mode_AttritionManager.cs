using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mode_AttritionManager : Singleton<Mode_AttritionManager>
{
    public int lead;
    public KillCounter leader;

    public List<KillCounter> killCounts;

    public GameObject killCounterObj;

    private void Awake()
    {
        blueTeamKills = 0;
        redTeamKills = 0;
        winningTeam = 0;
    }

    public void AddKill(int playerID)
    {
        killCounts[playerID].counter++;

        CheckKills(playerID);
    }

    int blueTeamKills;
    int redTeamKills;
    public int winningTeam;

    public void CheckKills(int playerID)
    {
        if (PlayerConfigurationManager.Instance.numOfTeams > 0)
        {
            lead = leader.counter;

            foreach (KillCounter killCounter in killCounts)
            {
                if (killCounter.counter > lead)
                {
                    leader = killCounter;
                }
            }
            UIManager.Instance.playerUI[playerID].text = killCounts[playerID].counter.ToString();

            if (PlayerConfigurationManager.Instance.playerConfigs[playerID].teamNum == 1)
            {
                blueTeamKills++;
            }
            if (PlayerConfigurationManager.Instance.playerConfigs[playerID].teamNum == 2)
            {
                redTeamKills++;
            }

            if (redTeamKills < blueTeamKills)
            {
                winningTeam = 1;
            }
            else if (blueTeamKills < redTeamKills)
            {
                winningTeam = 2;
            }
            else if (blueTeamKills == redTeamKills)
            {
                winningTeam = 0;
            }

            if (GameConfigurationManager.Instance.pointLimit > 0)
            {
                if (killCounts[playerID].counter >= GameModeManager.Instance.pointLimit)
                { GameModeManager.Instance.TeamGameOver(false, winningTeam); return; } //GameOver
            }
        }
        else
        {
            lead = leader.counter;

            foreach (KillCounter killCounter in killCounts)
            {
                if (killCounter.counter > lead)
                {
                    leader = killCounter;
                }
            }
            UIManager.Instance.playerUI[playerID].text = killCounts[playerID].counter.ToString();

            if (GameConfigurationManager.Instance.pointLimit > 0)
            {
                if (killCounts[playerID].counter >= GameModeManager.Instance.pointLimit)
                { GameModeManager.Instance.GameOver(false, leader.playerID); return; } //GameOver
            }
        }
    }

    public void SpawnObjs(PlayerConfiguration playerConfig)
    {
        var killCounter = Instantiate(killCounterObj, transform);
        killCounts.Add(killCounter.GetComponent<KillCounter>());
        killCounter.GetComponent<KillCounter>().playerID = playerConfig.PlayerIndex;

        UIManager.Instance.playerUI[playerConfig.PlayerIndex].text = "0";

        leader = killCounts[0];
    }
}
