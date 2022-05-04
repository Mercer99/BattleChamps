using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject popupUI;
    private Animator uiAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        uiAnimator = popupUI.GetComponent<Animator>();

        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        uiAnimator.SetBool("GameStart", true);
        yield return new WaitForSeconds(1.05f);
        GetComponent<GameTimer>().StartTimer();
        GetComponent<Mode_AttritionManager>().SpawnPlayers();
    }
}
