using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject popupUI;
    private Animator uiAnimator;

    public AudioSource musicSource;
    public AudioSource announcerSource;

    public AudioClip readyClip;

    // Start is called before the first frame update
    void Awake()
    {
        uiAnimator = popupUI.GetComponent<Animator>();
        announcerSource = GetComponent<AudioSource>();
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        uiAnimator.SetBool("GameStart", true);
        announcerSource.PlayOneShot(readyClip);
        yield return new WaitForSeconds(1.05f);
        GetComponent<GameTimer>().StartTimer();
        GetComponent<GameModeManager>().SpawnPlayers();
        musicSource.Play();
    }
}
