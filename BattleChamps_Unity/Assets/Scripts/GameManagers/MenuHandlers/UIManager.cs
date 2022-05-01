using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public bool gamePaused = false;
    public GameObject pauseMenuPanel;

    public List<GameObject> playerList = new List<GameObject>();

    public GameObject coloursPanel;
    public GameObject coloursPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenuPanel.SetActive(false);
        Pause_ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Pause Menu Functions
    public void Pause_ResumeGame()
    {
        gamePaused = false;
        Pause_PausePanelActive();
    }
    public void Pause_RestartGame()
    {
        SceneManager.LoadScene(0);
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
