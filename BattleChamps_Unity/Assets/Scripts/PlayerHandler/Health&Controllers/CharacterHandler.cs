using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class CharacterHandler : MonoBehaviour
{
    [Header("Initialising")]
    public Animator charAnimator;
    private CharacterController charController;
    public GameObject head;

    private PlayerControls controls;
    private PlayerConfiguration playerConfig;

    [HideInInspector]
    public UIManager uiManager;

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
    public Transform groundCheck; // Assign to Gameobject placed at players feet (checks if the feet are touching ground)
    public float groundDistance = 0.4f;
    public LayerMask groundMask; // Assign to Custom layer that includes the ground
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
    private WeaponHandler weaponHandler;

    public GameObject[] headAccessories;

    public GameObject[] bodyAccessories;
    #endregion

    public AnimationClip[] chargeAbilities;
    public AnimationClip[] quickAbilites;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    public GameObject currentWeapon;

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        currentWeapon = allWeapons[playerConfig.chosenWeapon];
        currentWeapon.SetActive(true);

        weaponHandler = currentWeapon.GetComponent<WeaponHandler>();

        headAccessories[playerConfig.chosenHeadAccessory].SetActive(true);
        bodyAccessories[playerConfig.chosenBodyAccessory].SetActive(true);
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.InGameActions.Movement.name)
        {
            OnMove(obj);
        }
        if (obj.action.name == controls.InGameActions.Aiming.name)
        {
            PlayerRotation(obj);
        }
        if (obj.action.name == controls.InGameActions.Dash.name)
        {
            OnDash(obj);
        }
        if (obj.action.name == controls.InGameActions.BasicAttack.name)
        {
            OnAttack(obj);
        }
        if (obj.action.name == controls.InGameActions.ShieldParry.name)
        {
            OnShield(obj);
        }
        if (obj.action.name == controls.InGameActions.Ability1.name)
        {
            OnAbility1(obj);
        }
        if (obj.action.name == controls.InGameActions.Ability2.name)
        {
            OnAbility2(obj);
        }
    }

    private void OnEnable()
    {
        applyGravity = true;

        shield.SetActive(false);

        charController = GetComponent<CharacterController>();

        currentSpeed = defaultSpeed;

        uiManager = UIManager.Instance;

        GetComponent<CharacterStats>().playerID = Mode_AttritionManager.Instance.allPlayers.Count;
        Mode_AttritionManager.Instance.PlayerJoined(gameObject);
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

    public void OnPause(CallbackContext context)
    { uiManager.Pause_TogglePause(); }

    public void OnMove(CallbackContext context)
    { movementInput = context.ReadValue<Vector2>(); }
    public void PlayerRotation(CallbackContext context)
    { rotationInput = -context.ReadValue<Vector2>(); }

    public void OnDash(CallbackContext context)
    {
        if (applyMovement() == false)
        { return; }
        if (dashed)
        { return; }

        if (isGrounded)
        {
            dashed = true;
            Debug.Log("DASHH");
            StartCoroutine(DashCoroutine());
        }
    }
    public void OnAttack(CallbackContext context)
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
    public void OnShield(CallbackContext context)
    {
        if (usingAbility || disabled)
        { return; }

        usingAbility = true;
        AnimLengthCheck();
        StartCoroutine(ShieldCoroutine());
    }
    public void OnAbility1(CallbackContext context)
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
    public void OnAbility2(CallbackContext context)
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
