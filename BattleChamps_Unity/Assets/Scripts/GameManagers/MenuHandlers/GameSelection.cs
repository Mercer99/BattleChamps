using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class Arena
{
    public string levelName;
    public GameObject arenaObj;
}
[System.Serializable]
public class GameMode
{
    public string gamemodeName;
    public string description;

    public float defaultLength;
    public int defaultPointLimit;
}

public class GameSelection : MonoBehaviour
{
    public Arena[] allArenas;
    public GameMode[] allGamemodes;
    public float[] gameLengths;
    public int[] pointLimits;

    public int currentLevelNum;

    public string arenaName;
    public string arenaSwitchName;

    public TextMeshProUGUI levelNameTextbox;

    public int currentGamemodeNum;

    public string gamemodeName;

    public TextMeshProUGUI gamemodeNameTextbox;
    public TextMeshProUGUI gamemodeDescriptionTextbox;

    public int currentLengthNum;

    public float gameLength;

    public TextMeshProUGUI gameLengthTextbox;

    public int currentPointLimitNum;

    public int pointLimit;

    public TextMeshProUGUI pointLimitTextbox;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var level in allArenas)
        { level.arenaObj.SetActive(false); }
        allArenas[currentLevelNum].arenaObj.SetActive(true);

        arenaSwitchName = allArenas[currentLevelNum].arenaObj.name;
        arenaName = allArenas[currentLevelNum].levelName;
        levelNameTextbox.text = arenaName;

        gamemodeName = allGamemodes[currentGamemodeNum].gamemodeName;
        gamemodeDescriptionTextbox.text = allGamemodes[currentGamemodeNum].description;

        gamemodeNameTextbox.text = gamemodeName;
        gameLength = allGamemodes[currentGamemodeNum].defaultLength;
        if (gameLength <= 0)
        { gameLengthTextbox.text = "Endless"; }
        else { gameLengthTextbox.text = gameLength.ToString("F0"); }

        pointLimit = allGamemodes[currentGamemodeNum].defaultPointLimit;
        if (pointLimit <= 0)
        { pointLimitTextbox.text = "No Limit"; }
        else { pointLimitTextbox.text = pointLimit.ToString("F0"); }
    }

    public void ChangeLevel()
    {
        currentLevelNum++;
        if (currentLevelNum == allArenas.Length)
        { currentLevelNum = 0; }

        foreach (var level in allArenas)
        { level.arenaObj.SetActive(false); }

        allArenas[currentLevelNum].arenaObj.SetActive(true);
        arenaSwitchName = allArenas[currentLevelNum].arenaObj.name;

        arenaName = allArenas[currentLevelNum].levelName;
        levelNameTextbox.text = arenaName;
    }

    public void ChangeGameMode()
    {
        currentGamemodeNum++;
        if (currentGamemodeNum == allGamemodes.Length)
        { currentGamemodeNum = 0; }

        gamemodeName = allGamemodes[currentGamemodeNum].gamemodeName;
        gamemodeDescriptionTextbox.text = allGamemodes[currentGamemodeNum].description;
        
        gamemodeNameTextbox.text = gamemodeName;

        gameLength = allGamemodes[currentGamemodeNum].defaultLength;
        gameLengthTextbox.text = gameLength.ToString("F0");
    }

    public void ChangeGameLength()
    {
        currentLengthNum++;
        if (currentLengthNum == gameLengths.Length)
        { currentLengthNum = 0; }

        gameLength = gameLengths[currentLengthNum];

        if (gameLength <= 0)
        { gameLengthTextbox.text = "Endless"; }
        else { gameLengthTextbox.text = gameLength.ToString("F0"); }
    }

    public void ChangePointLimit()
    {
        currentPointLimitNum++;
        if (currentPointLimitNum == pointLimits.Length)
        { currentPointLimitNum = 0; }

        pointLimit = pointLimits[currentPointLimitNum];

        if (pointLimit <= 0)
        { pointLimitTextbox.text = "NO LIMIT"; }
        else { pointLimitTextbox.text = pointLimit.ToString("F0"); }
    }

    public void ConfirmSettings()
    {
        GameObject config = GameObject.Find("GameConfigManager");
        GameConfigurationManager configManager = config.GetComponent<GameConfigurationManager>();

        configManager.gameLength = gameLength;
        configManager.currentGamemode = allGamemodes[currentGamemodeNum];
        configManager.levelName = arenaSwitchName;
        configManager.pointLimit = pointLimit;

        SceneManager.LoadScene(3);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(1);
    }
}
