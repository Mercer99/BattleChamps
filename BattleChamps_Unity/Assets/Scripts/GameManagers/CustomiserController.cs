using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomiserController : MonoBehaviour
{
    public bool playerJoined;
    public GameObject player;

    public GameObject playerPrefab;

    public Image joinedIndicator;
    public Sprite notjoinedImage;
    public Sprite notReadyImage;
    public Sprite readyImage;

    public GameObject menu1;
    public GameObject menu2;

    public GameObject[] menu1Options;
    public GameObject[] menu2Options;

    private int menuCounter;

    public GameObject currentOption;

    public bool menuSwapped;

    // Start is called before the first frame update
    void Awake()
    {
        menuCounter = 0;

        menuSwapped = false;
        playerJoined = false;
        joinedIndicator.sprite = notjoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);

        currentOption = menu1Options[menuCounter];
        currentOption.GetComponent<CustomiserMenuOptions>().Unselected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerJoined()
    {
        playerJoined = true;
        joinedIndicator.sprite = notReadyImage;
        currentOption.GetComponent<CustomiserMenuOptions>().Selected();
    }
    public void PlayerLeft()
    {
        menuSwapped = false;
        playerJoined = false;
        joinedIndicator.sprite = notjoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);

        menuCounter = 0;
        currentOption = menu1Options[menuCounter];
        currentOption.GetComponent<CustomiserMenuOptions>().Unselected();
    }
    public void playerReady()
    {
        joinedIndicator.sprite = readyImage;
    }
    public void playerUnready()
    {
        joinedIndicator.sprite = notReadyImage;
    }

    public void NewOptionSelected(GameObject newOption)
    {
        newOption.GetComponent<CustomiserMenuOptions>().Unselected();

        currentOption = newOption;
        currentOption.GetComponent<CustomiserMenuOptions>().Selected();
    }

    public void ScrollUp()
    {
        menuCounter--;
        if (menuSwapped)
        {
            if (menuCounter < 0)
            { menuCounter = menu2Options.Length - 1; }
            if (menuCounter >= menu2Options.Length)
            { menuCounter = 0; }
            NewOptionSelected(menu2Options[menuCounter]);
        }
        else
        {
            menuCounter--;
            if (menuCounter < 0)
            { menuCounter = menu1Options.Length - 1; }
            if (menuCounter >= menu1Options.Length)
            { menuCounter = 0; }
            NewOptionSelected(menu1Options[menuCounter]);
        }
    }
    public void ScrollDown()
    {

    }

    public void NextItem()
    {

    }
    public void PreviousItem()
    {

    }

    public void MenuSwap()
    {
        menuSwapped = !menuSwapped;
        menu1.SetActive(!menuSwapped);
        menu2.SetActive(menuSwapped);

        NewOptionSelected(menuSwapped ? menu2Options[0] : menu1Options[0]);
    }
}
