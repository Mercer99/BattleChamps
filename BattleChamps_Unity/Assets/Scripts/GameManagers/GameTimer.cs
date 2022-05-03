using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime;
    public float maxTime;

    GameObject config;
    public GameConfigurationManager configManager;

    void Awake()
    {
        if (GameObject.Find("GameConfigManager") != null)
        {
            config = GameObject.Find("GameConfigManager");
            configManager = config.GetComponent<GameConfigurationManager>();
            if (configManager.gameLength > 0)
            { currentTime = configManager.gameLength; }
            else { timerText.text = "ENDLESS"; }
        }
        else { currentTime = 60; }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("F0");
        }
        else if (currentTime <= 0)
        {
            timerText.text = "GAME OVER";
        }
    }
}
