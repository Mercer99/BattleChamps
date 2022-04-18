using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterHandler : MonoBehaviour
{
    private CharacterController charController;
    private Animator charAnimator;
    PlayerInput playerInput;
    public GameObject head;

    [HideInInspector]
    public UIManager uiManager;

    public PLAYER_VALUES playerValues;

    [Header("Player Values")]
    public float defaultSpeed = 12f;
    public float gravity = -9.81f;
    [HideInInspector]
    public string playerName;

    [Header("Dash")]
    public float dashCooldown = 2f;
    public float dashSpeed = 30;
    public float dashTime = 1;

    [Header("Gravity")]
    // Assign to Gameobject placed at players feet (checks if the feet are touching ground)
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    // Assign to Custom layer that includes the ground
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 rotationInput;

    private bool dashed = false;
    private bool attacking = false;
    private bool usingAbility = false;
    private float currentSpeed;
    private float animLength;

    private bool chargingAbility = false;
    public bool disabled = false;

    [HideInInspector]
    public bool applyGravity = true;
    public bool applyMovement()
    {
        if (chargingAbility || disabled)
        { return false; }

        return true;
    }

    #region Customisation Variables
    public GameObject shield;

    public GameObject[] allWeapons;
    private GameObject currentWeapon;
    private WeaponHandler weaponHandler;

    public GameObject[] headAccessories;
    private GameObject currentHeadAccessory;

    //public GameObject[] bodyAccessories;
    //private GameObject currentBodyAccessory;
    #endregion

    public AnimationClip[] chargeAbilities;
    public AnimationClip[] quickAbilites;

    private void OnEnable()
    {
        playerValues = GetComponent<PLAYER_VALUES>();

        applyGravity = true;

        shield.SetActive(false);

        charController = GetComponent<CharacterController>();
        charAnimator = GetComponent<Animator>();

        currentSpeed = defaultSpeed;

        uiManager = UIManager.Instance;

        GetComponent<CharacterStats>().playerID = Mode_AttritionManager.Instance.allPlayers.Count;
        Mode_AttritionManager.Instance.PlayerJoined(gameObject);

        foreach (GameObject weapon in allWeapons)
        { weapon.SetActive(false); }
        int randWeapon = Random.Range(0, allWeapons.Length);
        allWeapons[randWeapon].SetActive(true);
        currentWeapon = allWeapons[randWeapon];
        weaponHandler = currentWeapon.GetComponent<WeaponHandler>();

        foreach (GameObject hat in headAccessories)
        { hat.SetActive(false); }
        int randHeadAcc = Random.Range(0, headAccessories.Length);
        headAccessories[randHeadAcc].SetActive(true);
        currentHeadAccessory = headAccessories[randHeadAcc];
    }

    #region Inputs
    #region Check Current Animation
    public void AnimLengthCheck()
    {
        int animCheckCounter = 0;

        foreach (AnimationClip abilityAnim in chargeAbilities)
        {
            int animHash = Animator.StringToHash("Attack Layer.Shield");
            if (charAnimator.GetCurrentAnimatorStateInfo(1).fullPathHash == animHash)
            {
                animCheckCounter++;
            }
        }

        if (animCheckCounter > 0)
        {
            chargingAbility = true;
        }

        animLength = charAnimator.GetCurrentAnimatorStateInfo(1).length;
    }
    #endregion

    public void OnPause(InputAction.CallbackContext context)
    { uiManager.Pause_TogglePause(); }

    public void OnMove(InputAction.CallbackContext context)
    { movementInput = context.ReadValue<Vector2>(); }
    public void PlayerRotation(InputAction.CallbackContext context)
    { rotationInput = context.ReadValue<Vector2>(); }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (applyMovement() == false)
        { return; }
        if (dashed)
        { return; }

        if (isGrounded)
        {
            dashed = true;
            StartCoroutine(DashCoroutine());
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (disabled)
        { return; }

        if (usingAbility || attacking)
        { return; }

        attacking = true;
        weaponHandler.EnableCollider(true);
        AnimLengthCheck();
        GetComponent<PlayerSounds>().playAttackVoiceline();
        StartCoroutine(AttackCoroutine());
    }
    public void OnShield(InputAction.CallbackContext context)
    {
        if (usingAbility || disabled)
        { return; }

        usingAbility = true;
        AnimLengthCheck();
        StartCoroutine(ShieldCoroutine());
    }
    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (disabled == false)
        {
            if (!usingAbility)
            {
                AnimLengthCheck();
                StartCoroutine(AbilityCoroutine(1));
            }
        }
    }
    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (disabled == false)
        {
            if (!usingAbility)
            {
                AnimLengthCheck();
                StartCoroutine(AbilityCoroutine(2));
            }
        }
    }
    #endregion

    void Update()
    {
        if (applyGravity)
        { ApplyGravity(); }
        if (applyMovement())
        { ApplyMovement(); }

        HandleRotation();
    }

    #region Movement & Gravity
    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        float movementValue()
        {
            if (applyMovement() == false)
            { return 0; }

            if (movementInput.x != 0 || movementInput.y != 0)
            { return 1; }
            else { return 0; }
        }

        charAnimator.SetFloat("AnimFloatRunning", movementValue());

        if (applyMovement() == false)
        { return; }

        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        charController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        if (disabled)
        { return; }

        Vector3 lookDirection = new Vector3(rotationInput.x, 0, rotationInput.y).normalized;

        if (rotationInput != Vector2.zero)
        { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection).normalized, 0.15F); }
    }
    #endregion

    #region Ability Use Coroutines
    private IEnumerator DashCoroutine()
    {
        currentSpeed = dashSpeed;
        yield return new WaitForSecondsRealtime(dashTime);
        currentSpeed = defaultSpeed;

        GetComponent<PlayerUI_Handler>().DashCDTimer(dashCooldown);

        yield return new WaitForSeconds(dashCooldown);
        dashed = false;
    }
    private IEnumerator AttackCoroutine()
    {
        charAnimator.SetTrigger("AnimTriggerAttacking");
        
        yield return new WaitForSecondsRealtime(animLength);
        attacking = false;
        weaponHandler.EnableCollider(false);
    }
    private IEnumerator ShieldCoroutine()
    {
        shield.SetActive(true);
        GetComponent<CharacterStats>().canBeDamaged = false;
        charAnimator.SetTrigger("AnimTriggerShield");
        GetComponent<PlayerUI_Handler>().ShieldCDTimer(animLength);
        chargingAbility = true;

        yield return new WaitForSecondsRealtime(animLength);

        chargingAbility = false;
        shield.SetActive(false);
        GetComponent<CharacterStats>().canBeDamaged = true;
        usingAbility = false;
    }

    private IEnumerator AbilityCoroutine(int abilityNum)
    {
        if (usingAbility)
        { yield break; }
        
        if (abilityNum == 1)
        { GetComponent<PlayerUI_Handler>().Ability1CDTimer(animLength); }
        else if (abilityNum != 1)
        { GetComponent<PlayerUI_Handler>().Ability2CDTimer(animLength); }

        usingAbility = true;
        yield return new WaitForSecondsRealtime(animLength);
        chargingAbility = false;
        usingAbility = false;
    }
    #endregion
}
