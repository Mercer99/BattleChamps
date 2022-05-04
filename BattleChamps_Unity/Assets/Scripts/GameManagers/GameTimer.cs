using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTimer : MonoBehaviour
{
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
        }
        else { currentTime = 60; timerText.text = currentTime.ToString("F0"); }
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
                timerText.text = "GAME OVER";
            }
        }
    }
}
