using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class CustomiserController : MonoBehaviour
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI headName;
    public TextMeshProUGUI bodyName;

    public TextMeshProUGUI ability1Name;
    public TextMeshProUGUI ability2Name;

    public PlayerConfiguration thisConfig;

    public bool playerJoined;
    public bool playerReady;
    public GameObject player;

    public Image joinedIndicator;
    public Sprite notJoinedImage;
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

    public int currentAbility1Num;
    public Ability_Base currentAbility1;
    public int abilityAmount1;

    public int currentAbility2Num;
    public Ability_Base currentAbility2;
    public int abilityAmount2;

    public AudioSource buttonSoundsSource;
    public AudioSource voicelinesSource;
    public AudioClip[] readyVoicelines;
    public AudioClip swing;
    public AudioClip click;

    public Image playerColourIm;

    private PlayerControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        playerJoined = false;
        joinedIndicator.sprite = notJoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);

        controls = new PlayerControls();
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        thisConfig = config;
        currentAbility1 = itemManager.abilities1[0];
        currentAbility2 = itemManager.abilities2[0];
        config.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.MenuActions.Navigate.name)
        {
            if (inputDisabled)
            { return; }

            Vector2 scrollValue = obj.ReadValue<Vector2>();

            if (obj.action.WasPressedThisFrame())
            {
                if (scrollValue.y > 0)
                {
                    onPressUp();
                }
                else if (scrollValue.y < 0)
                {
                    onPressDown();
                }
            }

            if (obj.action.WasReleasedThisFrame())
            {
                onReleaseUp();
                onReleaseDown();
                if (scrollValue.y > 0)
                {
                    onReleaseUp();
                }
                else if (scrollValue.y < 0)
                {
                    onReleaseDown();
                }
            }

            if (obj.action.WasPressedThisFrame())
            {
                if (inputDisabled)
                { return; }

                if (scrollValue.x > 0)
                {
                    onPressRight();
                }
                else if (scrollValue.x < 0)
                {
                    onPressLeft();
                }
            }

            if (obj.action.WasReleasedThisFrame())
            {
                onReleaseLeft();
                onReleaseRight();
                if (scrollValue.x > 0)
                {
                    onReleaseRight();
                }
                else if (scrollValue.x < 0)
                {
                    onReleaseLeft();
                }
            }
        }
        if (obj.action.name == controls.MenuActions.Submit.name)
        {
            if (CustomisationScreenManager.Instance.activated)
            {
                CustomisationScreenManager.Instance.QuitToMenu();
            }
            else
            {
                if (playerReady == false)
                {
                    playerReady = true;
                    PlayerReady();
                }
            }
        }
        if (obj.action.name == controls.MenuActions.Cancel.name)
        {
            if (obj.performed)
            {
                if (playerReady)
                {
                    playerReady = false;
                    PlayerUnready();
                    return;
                }
                else
                {
                    if (CustomisationScreenManager.Instance.activated)
                    {
                        CustomisationScreenManager.Instance.BackToGame();
                    }
                    else
                    {
                        Color colour = playerColourIm.color;
                        CustomisationScreenManager.Instance.BackOut(thisConfig, colour);
                    }
                }
            }
        }
        if (obj.action.name == controls.MenuActions.ChangeMenu.name)
        {
            if (inputDisabled)
            { return; }

            if (obj.performed)
            { MenuSwap(); }
        }
        if (obj.action.name == controls.MenuActions.ShuffleLeft.name)
        {
            if (inputDisabled)
            { return; }

            if (obj.performed)
            { PreviousItem(); }
        }
        if (obj.action.name == controls.MenuActions.ShuffleRight.name)
        {
            if (inputDisabled)
            { return; }

            if (obj.performed)
            { NextItem(); }
        }
    }

    #region Scroll Inputs
    public int index;
    public int maxIndex;

    public int indexS;
    public int maxIndexS;
    [SerializeField] bool keyDown;
    [SerializeField] bool keyDownS;
    public bool isPressUp, isPressDown;
    public bool isPressLeft, isPressRight;
    int VerticalMovement;
    int HorizontalMovement;

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

    public void onPressLeft()
    {
        isPressLeft = true;
    }

    public void onReleaseLeft()
    {
        isPressLeft = false;
    }

    public void onPressRight()
    {
        isPressRight = true;
    }

    public void onReleaseRight()
    {
        isPressRight = false;
    }
    #endregion

    #region Start & Update
    void Start()
    {
        maxIndex = 1;
        maxIndexS = 1;
        playerReady = false;
        isPressUp = isPressDown = false;
        isPressLeft = isPressRight = false;

        itemManager = player.GetComponent<PlayerItemsManager>();
        weaponAmount = itemManager.weapons.Length - 1;
        headAccessoryAmount = itemManager.headAccessories.Length - 1;
        bodyAccessoryAmount = itemManager.bodyAccessories.Length - 1;

        abilityAmount1 = itemManager.abilities1.Length - 1;
        abilityAmount2 = itemManager.abilities2.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        VerticalMove();
        HorizontalMove();

        weaponName.text = itemManager.weaponName;
        headName.text = itemManager.headAccName;
        bodyName.text = itemManager.bodyAccName;
        ability1Name.text = itemManager.ability1Name;
        ability2Name.text = itemManager.ability2Name;
    }

    void VerticalMove()
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
    }

    void HorizontalMove()
    {
        if (isPressRight) HorizontalMovement = 1;
        if (isPressLeft) HorizontalMovement = -1;
        if (!isPressLeft && !isPressRight) HorizontalMovement = 0;

        if (HorizontalMovement != 0)
        {
            if (!keyDownS)
            {
                if (HorizontalMovement < 0)
                {
                    PreviousItem();
                }
                else if (HorizontalMovement > 0)
                {
                    PreviousItem();
                }

                keyDownS = true;
            }
        }
        else
        {
            keyDownS = false;
        }
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
        joinedIndicator.sprite = notJoinedImage;

        menu1.SetActive(true);
        menu2.SetActive(false);
    }
    public void PlayerReady()
    {
        if (menuSwapped)
        { MenuSwap(); }

        joinedIndicator.sprite = readyImage;
        player.GetComponent<Animator>().SetBool("AnimBoolPlayerReady", true);

        playerReady = true;
        inputDisabled = true;

        int randomVL = Random.Range(0, readyVoicelines.Length - 1);
        voicelinesSource.PlayOneShot(readyVoicelines[randomVL]);
        GetComponent<PlayerSetupMenuController>().SelectItems();
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
        else
        {
            if (index == 0)
            {
                currentAbility1Num++;
                CheckAbility1(true);
            }
            else if (index == 1)
            {
                currentAbility2Num++;
                CheckAbility2(true);
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
        else
        {
            if (index == 0)
            {
                currentAbility1Num--;
                CheckAbility1(false);
            }
            else if (index == 1)
            {
                currentAbility2Num--;
                CheckAbility2(false);
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
    public void CheckAbility1(bool next)
    {
        if (currentAbility1Num < 0)
        { currentAbility1Num = abilityAmount1; }
        else if (currentAbility1Num > abilityAmount1)
        { currentAbility1Num = 0; }
        itemManager.ability1Num = currentAbility1Num;

        itemManager.ChangeAbility1(); currentAbility1 = itemManager.abilities1[currentAbility1Num];

        if (itemManager.ability1Name == itemManager.ability2Name)
        {
            if (next)
            {
                currentAbility1Num++;
                CheckAbility1(true);
            }
            else
            {
                currentAbility1Num--;
                CheckAbility1(false);
            }
        }
    }
    public void CheckAbility2(bool next)
    {
        if (currentAbility2Num < 0)
        { currentAbility2Num = abilityAmount2; }
        else if (currentAbility2Num > abilityAmount2)
        { currentAbility2Num = 0; }
        itemManager.ability2Num = currentAbility2Num;

        itemManager.ChangeAbility2(); currentAbility2 = itemManager.abilities2[currentAbility2Num];

        if (itemManager.ability1Name == itemManager.ability2Name)
        {
            if (next)
            {
                currentAbility2Num++;
                CheckAbility2(true);
            }
            else
            {
                currentAbility2Num--;
                CheckAbility2(false);
            }
        }
    }

    #endregion

    #region Menu Transitions
    public void MenuSwap()
    {
        if (inputDisabled)
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
