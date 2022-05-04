using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour//Singleton<MainMenuHandler>
{
    public GameObject playerObj;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public bool creditsActive;
    public bool optionsActive;
    public bool menuActive;

    void Awake()
    {
        creditsActive = false;
        optionsActive = false;
        menuActive = true;

        if (GameObject.Find("ConfigManager") != null)
        { Destroy(GameObject.Find("ConfigManager"));  }
        if (GameObject.Find("GameConfigManager") != null)
        { Destroy(GameObject.Find("GameConfigManager")); }

        playerObj.SetActive(true);
        GetComponent<PlayerInputManager>().enabled = true;
    }

    public void BackToMain()
    {
        if (menuActive)
        { return; }

        StartCoroutine(MainMenu());
    }
    IEnumerator MainMenu()
    {
        if (creditsActive)
        { creditsPanel.GetComponent<MenuPanels>().SlideOut(); }
        else if (optionsActive)
        { optionsPanel.GetComponent<MenuPanels>().SlideOut(); }
        yield return new WaitForSeconds(0.2f);
        mainPanel.GetComponent<MenuPanels>().SlideIn();
        creditsActive = false;
        optionsActive = false;
    }

    public void Options()
    { StartCoroutine(OptionsSwitch()); }
    IEnumerator OptionsSwitch()
    {
        optionsActive = true;
        menuActive = false;
        mainPanel.GetComponent<MenuPanels>().SlideOut();
        yield return new WaitForSeconds(0.2f);
        optionsPanel.GetComponent<MenuPanels>().SlideIn();
    }
    public void Credits()
    { StartCoroutine(CreditsSwitch()); }
    IEnumerator CreditsSwitch()
    {
        creditsActive = true;
        menuActive = false;
        mainPanel.GetComponent<MenuPanels>().SlideOut();
        yield return new WaitForSeconds(0.2f);
        creditsPanel.GetComponent<MenuPanels>().SlideIn();
    }

    public void LoadLevel(int levelNum)
    { SceneManager.LoadScene(levelNum); }

    public void QuitGame()
    { Application.Quit(); }
}
