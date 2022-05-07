using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CustomisationScreenManager : Singleton<CustomisationScreenManager>
{
    public GameObject configManager;
    public GameObject backOutPanel;
    public bool activated;
    public bool deleted;

    private float readyMenuCurrentTimer;
    public float readyMenuMaxTime = 10;
    public GameObject timerPanel;
    bool countdownStarted;

    public Image timer;
    public TextMeshProUGUI timerText;

    public void Update()
    {
        if (readyMenuCurrentTimer > 0)
        { 
            readyMenuCurrentTimer -= Time.deltaTime;
            timerText.text = readyMenuCurrentTimer.ToString("F1");
            timer.fillAmount = readyMenuCurrentTimer / readyMenuMaxTime;
        }
        else if (readyMenuCurrentTimer < 0)
        { readyMenuCurrentTimer = 0; }

        if (readyMenuCurrentTimer <= 0)
        { 
            if (countdownStarted)
            { countdownStarted = false;  SceneManager.LoadScene("MENU_TeamSelection"); }
        }
    }

    public void StartCountdown()
    {
        timerPanel.SetActive(true);
        countdownStarted = true;
        readyMenuCurrentTimer = readyMenuMaxTime;
    }
    public void StopCountdown()
    {
        readyMenuCurrentTimer = 0;
        countdownStarted = false;
        timerPanel.SetActive(false);
    }

    public void BackOut(PlayerConfiguration config, Color color)
    {
        if (readyMenuCurrentTimer <= 0)
        {
            backOutPanel.GetComponent<BackOutScreen>().StartPanel(color, config.PlayerIndex);

            activated = true;
            PlayerConfigurationManager.Instance.ShowBackOutMenu(config.PlayerIndex);
        }
    }

    public void BackToGame()
    { backOutPanel.GetComponent<BackOutScreen>().DisablePause(); activated = false; }

    public void QuitToMenu()
    { SceneManager.LoadScene(0); }
}
