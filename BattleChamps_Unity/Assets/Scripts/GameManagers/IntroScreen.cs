using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
   // Update is called once per frame
    void Start()
    {
        Invoke("StartGame", 8.5f);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
