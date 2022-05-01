using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuHandler : Singleton<MainMenuHandler>
{
    public GameObject playerObj;

    void Start()
    {
        if (GameObject.Find("ConfigManager") != null)
        { Destroy(GameObject.Find("ConfigManager")); }

        GetComponent<PlayerInputManager>().enabled = true;
        playerObj.SetActive(true);
    }

    void Update()
    {

    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum);
    }

    public void SlideIn()
    {
        GetComponent<Animator>().SetTrigger("");
    }

    public void SlideOut()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
