using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomiserController : MonoBehaviour
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI headName;
    public TextMeshProUGUI bodyName;

    public bool playerJoined;
    public bool playerReady;
    public GameObject player;

    public Image joinedIndicator;
    public Sprite notjoinedImage;
    public Sprite notReadyImage;
    public Sprite readyImage;

    public GameObject menu1;
    public GameObject menu2;
    public GameObject[] menuOptions;
    public bool menuSwapped;
    public bool inputDisabled;

    public int currentHeadAccessory;
    public int headAccessoryAmount;

    public int currentBodyAccessory;
    public int bodyAccessoryAmount;

    public int currentWeapon;
    public int weaponAmount;

    public AudioSource buttonSoundsSource;
    public AudioSource voicelinesSource;
    public AudioClip[] readyVoicelines;
    public AudioClip swing;
    public AudioClip click;

    // Start is called before the first frame update
    void Awake()
    {
        playerJoined = false;
        joinedIndicator.sprite = notjoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);
    }

    #region Scroll Inputs
    public int index;
    public int maxIndex;
    [SerializeField] bool keyDown;
    public bool isPressUp, isPressDown;
    int VerticalMovement;

    PlayerItemsManager itemManager;

    public void onPressUp()
    {
        isPressUp = true;
    }

    public void onReleaseUp()
    {
        isPressUp = false;
    }

    public void onPressDown()
    {
        isPressDown = true;
    }

    public void onReleaseDown()
    {
        isPressDown = false;
    }
    #endregion

    #region Start & Update
    void Start()
    {
        maxIndex = 1;
        playerReady = false;
        isPressUp = isPressDown = false;

        itemManager = player.GetComponent<PlayerItemsManager>();
        weaponAmount = itemManager.weapons.Length - 1;
        headAccessoryAmount = itemManager.headAccessories.Length - 1;
        bodyAccessoryAmount = itemManager.bodyAccessories.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressUp) VerticalMovement = 1;
        if (isPressDown) VerticalMovement = -1;
        if (!isPressUp && !isPressDown) VerticalMovement = 0;

        if (VerticalMovement != 0)
        {
            if (!keyDown)
            {
                if (VerticalMovement < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }

                }
                else if (VerticalMovement > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }

                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }

        weaponName.text = itemManager.weaponName;
        headName.text = itemManager.headAccName;
        bodyName.text = itemManager.bodyAccName;
    }
    #endregion

    #region Join & Ready Handlers
    public void PlayerJoined()
    {
        playerJoined = true;
        joinedIndicator.sprite = notReadyImage;
        player.GetComponent<Animator>().SetBool("AnimBoolPlayerActive", true);
    }
    public void PlayerLeft()
    {
        PlayerUnready();
        player.GetComponent<Animator>().SetBool("AnimBoolPlayerActive", false);
        playerJoined = false;
        joinedIndicator.sprite = notjoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);
    }
    public void PlayerReady()
    {
        if (inputDisabled) { return; }
        joinedIndicator.sprite = readyImage;
        player.GetComponent<Animator>().SetBool("AnimBoolPlayerReady", true);
        if (menuSwapped)
        { MenuSwap(); }
        playerReady = true;
        inputDisabled = true;

        int randomVL = Random.Range(0, readyVoicelines.Length - 1);
        voicelinesSource.PlayOneShot(readyVoicelines[randomVL]);
        GetComponent<PlayerSetupMenuController>().ReadyPlayer();
    }
    public void PlayerUnready()
    {
        joinedIndicator.sprite = notReadyImage;
        player.GetComponent<Animator>().SetBool("AnimBoolPlayerReady", false);
        playerReady = false;
        inputDisabled = false;
        GetComponent<PlayerSetupMenuController>().UnreadyPlayer();
    }
    #endregion

    public void NextItem()
    {
        buttonSoundsSource.PlayOneShot(click);

        if (menuSwapped)
        {
            if(index == 0)
            {
                currentWeapon++;
                CheckWeapon();
            }
            else if (index == 1)
            {
                currentHeadAccessory++;
                CheckHeadAcc();
            }
            else if (index == 2)
            {
                currentBodyAccessory++;
                CheckBodyAcc();
            }
        }
    }
    public void PreviousItem()
    {
        buttonSoundsSource.PlayOneShot(click);

        if (menuSwapped)
        {
            if (index == 0)
            {
                currentWeapon--;
                CheckWeapon();
            }
            else if (index == 1)
            {
                currentHeadAccessory--;
                CheckHeadAcc();
            }
            else if (index == 2)
            {
                currentBodyAccessory--;
                CheckBodyAcc();
            }
        }
    }

    #region ItemChecks
    public void CheckWeapon()
    {
        if (currentWeapon < 0)
        { currentWeapon = weaponAmount; }
        else if (currentWeapon > weaponAmount)
        { currentWeapon = 0; }
        itemManager.weaponNum = currentWeapon;
        itemManager.ChangeWeapon();
    }
    public void CheckHeadAcc()
    {
        if (currentHeadAccessory < 0)
        { currentHeadAccessory = headAccessoryAmount; }
        else if (currentHeadAccessory > headAccessoryAmount)
        { currentHeadAccessory = 0; }
        itemManager.headAccNum = currentHeadAccessory;
        itemManager.ChangeHead();
    }
    public void CheckBodyAcc()
    {
        if (currentBodyAccessory < 0)
        { currentBodyAccessory = bodyAccessoryAmount; }
        else if (currentBodyAccessory > bodyAccessoryAmount)
        { currentBodyAccessory = 0; }
        itemManager.bodyAccNum = currentBodyAccessory;
        itemManager.ChangeBody();
    }
    #endregion

    #region Menu Transitions
    public void MenuSwap()
    {
        if (playerReady)
        { return; }

        menuSwapped = !menuSwapped;
        StartCoroutine(menuSwap());
    }
    IEnumerator menuSwap()
    {
        MenuTransitionAnim(menuSwapped);

        buttonSoundsSource.PlayOneShot(swing);

        foreach (GameObject option in menuOptions)
        {
            option.GetComponent<CustomiserMenuOptions>().Unselected();
        }

        maxIndex = menuSwapped ? 2 : 1;
        index = 0;

        // Input re enabled using Event in Menu Animation - inputDisabled = false;
        yield return new WaitForSeconds(0.55f);
        if (playerReady)
        { inputDisabled = true; }
    }
    public void MenuTransitionAnim(bool swapped)
    { GetComponent<Animator>().Play(swapped ? "ToggleMenu" : "MenuReverse"); }
    public void InputDisable()
    { inputDisabled = true; }
    public void InputReenable()
    { inputDisabled = false; }
    #endregion
}
