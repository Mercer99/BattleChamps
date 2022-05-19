using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode_ConquestManager : Singleton<Mode_ConquestManager>
{
    public int lead;
    public KillCounter leader;

    public List<KillCounter> pointCounts;

    public GameObject pointCounterObj;

    private void Awake()
    {
        blueTeamPoints = 0;
        redTeamPoints = 0;
        winningTeam = 0;
    }

    public void AddPoints(int playerID)
    {
        pointCounts[playerID].counter++;

        CheckPoints(playerID);
    }

    int blueTeamPoints;
    int redTeamPoints;
    public int winningTeam;

    public void CheckPoints(int playerID)
    {
        if (PlayerConfigurationManager.Instance.numOfTeams > 0)
        {
            lead = leader.counter;

            foreach (KillCounter killCounter in pointCounts)
            {
                if (killCounter.counter > lead)
                {
                    leader = killCounter;
                }
            }
            UIManager.Instance.playerUI[playerID].text = pointCounts[playerID].counter.ToString();

            if (PlayerConfigurationManager.Instance.playerConfigs[playerID].teamNum == 1)
            {
                blueTeamPoints++;
            }
            if (PlayerConfigurationManager.Instance.playerConfigs[playerID].teamNum == 2)
            {
                redTeamPoints++;
            }

            if (redTeamPoints < blueTeamPoints)
            {
                winningTeam = 1;
            }
            else if (blueTeamPoints < redTeamPoints)
            {
                winningTeam = 2;
            }
            else if (blueTeamPoints == redTeamPoints)
            {
                winningTeam = 0;
            }

            if (GameModeManager.Instance.pointLimit > 0)
            {
                if (pointCounts[playerID].counter >= GameModeManager.Instance.pointLimit)
                { GameModeManager.Instance.TeamGameOver(false, winningTeam); return; } //GameOver
            }
        }
        else
        {
            lead = leader.counter;

            foreach (KillCounter killCounter in pointCounts)
            {
                if (killCounter.counter > lead)
                {
                    leader = killCounter;
                }
            }
            UIManager.Instance.playerUI[playerID].text = pointCounts[playerID].counter.ToString();

            if (GameConfigurationManager.Instance.pointLimit > 0)
            {
                if (pointCounts[playerID].counter >= GameModeManager.Instance.pointLimit)
                { GameModeManager.Instance.GameOver(false, leader.playerID); return; } //GameOver
            }
        }
    }

    public void SpawnNewPoint()
    {

    }

    public void SpawnObjs(PlayerConfiguration playerConfig)
    {
        var killCounter = Instantiate(pointCounterObj, transform);
        pointCounts.Add(killCounter.GetComponent<KillCounter>());
        killCounter.GetComponent<KillCounter>().playerID = playerConfig.PlayerIndex;

        UIManager.Instance.playerUI[playerConfig.PlayerIndex].text = "0";

        leader = pointCounts[0];
    }
}
