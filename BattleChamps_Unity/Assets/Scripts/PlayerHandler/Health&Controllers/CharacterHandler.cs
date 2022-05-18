using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharacterHandler : MonoBehaviour
{
    [Header("Initialising")]
    public Animator charAnimator;
    private CharacterController charController;
    public Material[] playerMaterials;
    public SkinnedMeshRenderer playerMesh;

    private PlayerControls controls;
    public PlayerConfiguration playerConfig;
    public bool playerDead;

    [HideInInspector]
    public UIManager uiManager;

    [Header("Player Values")]
    public float defaultSpeed = 12f;
    public float gravity = -9.81f;
    [HideInInspector]
    public string playerName;
    private float currentShieldCD;
    public float shieldCD = 5f;
    public float currentStunDuration = 0f;

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
    public bool attacking = false;
    private bool usingAbility = false;
    public bool resetDisable = false;

    private float currentSpeed;
    private float animLength;

    public float attackComboBufferTime = 0.2f;
    public float basicAttackCD = 0.5f;
    public float basicAttackTimer = 0;
    [SerializeField] private float basicAttackTimerSet;
    public int comboHits;

    public bool removeRotation;

    public Transform notificationSpawnPoint;
    public GameObject notification;

    public bool chargingAbility = false;
    public bool disabled = false;

    public ParticleSystem dustParticles;

    [HideInInspector]
    public bool applyGravity = true;
    public bool applyMovement()
    {
        if (chargingAbility || disabled)
        { return false; }

        return true;
    }
    public void ResetDisable()
    {
        disabled = false;
    }

    #region Customisation Variables
    [Header("Accessories & Weapons")]
    public GameObject shield;

    public GameObject[] allWeapons;
    public WeaponHandler weaponHandler;

    public GameObject[] headAccessories;

    public GameObject[] bodyAccessories;
    #endregion

    public GameObject currentWeapon;
    public GameObject abilityHolderObj1;
    public GameObject abilityHolderObj2;
    private AbilityHolder abilityHolder1;
    private AbilityHolder abilityHolder2;

    #region Initialising & Update

    private void Awake()
    { controls = new PlayerControls(); }

    private void Start()
    {
        applyGravity = true;
        shield.SetActive(false);
        charController = GetComponent<CharacterController>();
        currentSpeed = defaultSpeed;
        uiManager = UIManager.Instance;
        GameModeManager.Instance.PlayerJoined(gameObject);
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;

        Debug.Log(config.teamNum);
        
        currentWeapon = allWeapons[playerConfig.chosenWeapon];
        currentWeapon.SetActive(true);

        weaponHandler = currentWeapon.GetComponent<WeaponHandler>();

        headAccessories[playerConfig.chosenHeadAccessory].SetActive(true);
        bodyAccessories[playerConfig.chosenBodyAccessory].SetActive(true);

        abilityHolder1 = abilityHolderObj1.GetComponent<AbilityHolder>();
        abilityHolder2 = abilityHolderObj2.GetComponent<AbilityHolder>();
        abilityHolder1.ability = playerConfig.chosenAbility1;
        abilityHolder2.ability = playerConfig.chosenAbility2;

        playerConfig.Input.onActionTriggered += Input_onActionTriggered;

        Material[] mats = playerMesh.materials;
        mats[0] = playerMaterials[playerConfig.PlayerIndex];
        playerMesh.materials = mats;

        GetComponent<CharacterStats>().playerID = config.PlayerIndex;
        GetComponent<PlayerUI_Handler>().teamNum = config.teamNum;
        GetComponent<CharacterStats>().teamNumber = config.teamNum;
        GetComponent<CharacterStats>().InitializePlayerStats();

        GetComponent<ControllerRumbler>().InitializeRumbler();
    }

    void Update()
    {
        if (applyGravity)
        { ApplyGravity(); }
        if (applyMovement() == true)
        { ApplyMovement(); }

        if (!removeRotation)
        { HandleRotation(); }

        if (abilityHolder1.state == AbilityHolder.AbilityState.active || abilityHolder2.state == AbilityHolder.AbilityState.active || shield.activeInHierarchy || currentSpeed != defaultSpeed)
        { usingAbility = true; }
        else { usingAbility = false; }

        WhirlwindAbility();

        if (currentShieldCD > 0)
        { currentShieldCD -= Time.deltaTime; }
        else { currentShieldCD = 0; }

        if (basicAttackTimer > 0)
        { basicAttackTimer -= Time.deltaTime; }
        else if (basicAttackTimer <= basicAttackTimerSet )
        {
            if (basicAttackTimer <= 0)
            { comboHits = 0; }
        }
        else
        { comboHits = 0; }
    }
    #endregion

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

    #region Inputs
    #region Check Current Animation
    public float AnimLengthCheck()
    {
        int animCheckCounter = 0;

        if (animCheckCounter > 0)
        {
            chargingAbility = true;
        }

        return animLength = charAnimator.GetCurrentAnimatorStateInfo(1).length;
    }
    #endregion

    public void OnPause(CallbackContext context)
    { uiManager.Pause_TogglePause(); }

    public void OnMove(CallbackContext context)
    { movementInput = context.ReadValue<Vector2>(); }

    public bool rotating;
    public void PlayerRotation(CallbackContext context)
    { rotationInput = -context.ReadValue<Vector2>(); rotating = context.performed; }

    public void SpawnNotification(string popupText)
    {
        GameObject popup = Instantiate(notification, notificationSpawnPoint);
        popup.GetComponent<PlayerUIStateNotification>().Popup(popupText);
    }

    public void OnDash(CallbackContext context)
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
    public void OnAttack(CallbackContext context)
    {
        if (disabled)
        { return; }

        if (usingAbility || attacking)
        { return; }

        if (context.performed)
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    public void OnShield(CallbackContext context)
    {
        if (usingAbility || disabled)
        { return; }

        if (currentShieldCD <= 0)
        {
            usingAbility = true;
            AnimLengthCheck();
            StartCoroutine(ShieldCoroutine());
        }
    }
    public void OnAbility1(CallbackContext context)
    {
        if (usingAbility || disabled)
        { return; }

        if (abilityHolder1.state != AbilityHolder.AbilityState.ready)
        { return; }
        float abilityCooldown = abilityHolder1.ability.activeTime + abilityHolder1.ability.cooldownTime;
        abilityHolder1.Activate();

        StartCoroutine(AbilityCoroutine1(abilityCooldown, abilityHolder1.ability.activeTime));

        if (abilityHolder1.ability.abilityName == "Whirlwind")
        { Invoke("DeactivateWhirlwind", abilityHolder1.ability.activeTime); }
    }
    public void OnAbility2(CallbackContext context)
    {
        if (usingAbility || disabled)
        { return; }

        if (abilityHolder2.state != AbilityHolder.AbilityState.ready)
        { return; }
        float abilityCooldown = abilityHolder2.ability.activeTime + abilityHolder2.ability.cooldownTime;
        abilityHolder2.Activate();

        StartCoroutine(AbilityCoroutine2(abilityCooldown, abilityHolder2.ability.activeTime));

        if (abilityHolder2.ability.abilityName == "Whirlwind")
        { Invoke("DeactivateWhirlwind", abilityHolder2.ability.activeTime); }
    }
    #endregion

    #region Movement & Gravity
    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        { velocity.y = -2f; }

        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }
    public Vector3 moveDirection;
    private void ApplyMovement()
    {
        dustParticles.Play();

        float movementValue()
        {
            if (applyMovement() == false)
            { return 0; }

            if (movementInput.x != 0 || movementInput.y != 0)
            { return 1; }
            else { return 0; }
        }

        if (movementValue() == 0)
        { dustParticles.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.1f, 0.1f, 0.1f), 1f); }
        else { dustParticles.transform.localScale = new Vector3(1, 1, 1); }

        charAnimator.SetFloat("AnimFloatRunning", movementValue());

        if (applyMovement() == false)
        { return; }

        moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        charController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
    public GameObject rotationIndicator;
    private void HandleRotation()
    {
        if (disabled)
        { return; }

        Vector3 lookDirection;

        lookDirection = new Vector3(rotationInput.x, 0, rotationInput.y).normalized;

        if (rotationInput != Vector2.zero)
        { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection).normalized, 0.15F); }
        else if (movementInput != Vector2.zero) 
        { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-moveDirection).normalized, 0.15F); }

        rotationIndicator.transform.rotation = gameObject.transform.rotation;
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
        if (attacking == false)
        {
            switch (comboHits)
            {
                case 0:
                    FirstHit();
                    attacking = true;
                    weaponHandler.EnableCollider(true);
                    GetComponent<PlayerSounds>().playAttackVoiceline();
                    yield return new WaitForSecondsRealtime(AnimLengthCheck()*0.5f);
                    weaponHandler.EnableCollider(false);
                    attacking = false;
                    break;
                case 1:
                    SecondHit();
                    attacking = true;
                    weaponHandler.EnableCollider(true);
                    GetComponent<PlayerSounds>().playAttackVoiceline();
                    yield return new WaitForSecondsRealtime(AnimLengthCheck()*0.5f);
                    weaponHandler.EnableCollider(false);
                    attacking = false;
                    break;
                case 2:
                    FinalHit();
                    attacking = true;
                    weaponHandler.EnableCollider(true);
                    GetComponent<PlayerSounds>().playAttackVoiceline();
                    yield return new WaitForSecondsRealtime(AnimLengthCheck() + basicAttackCD);
                    weaponHandler.EnableCollider(false);
                    attacking = false;
                    break;
            }
        }
    }
    void FirstHit()
    {
        print("First Hit");
        charAnimator.Play("IG_BasicMeleeAttack");
        comboHits++;
        basicAttackTimer = AnimLengthCheck() + attackComboBufferTime;
    }
    void SecondHit()
    {
        print("Second Hit");
        charAnimator.Play("IG_BasicMeleeAttackP2");
        comboHits++;
        basicAttackTimer = AnimLengthCheck() + attackComboBufferTime;
    }
    void FinalHit()
    {
        print("Third Hit");
        charAnimator.Play("IG_BasicMeleeAttackP3");
        comboHits = 0;
        basicAttackTimer = 0;
    }

    private IEnumerator ShieldCoroutine()
    {
        weaponHandler.EnableCollider(false);
        attacking = false;

        shield.SetActive(true);
        GetComponent<CharacterStats>().canBeDamaged = false;
        charAnimator.Play("A_Shield");
        currentShieldCD = shieldCD;
        GetComponent<PlayerUI_Handler>().ShieldCDTimer(currentShieldCD);

        yield return new WaitForSecondsRealtime(animLength);
        shield.SetActive(false);
        GetComponent<CharacterStats>().canBeDamaged = true;
    }

    private IEnumerator AbilityCoroutine1(float cooldown, float activeTime)
    {
        GetComponent<PlayerUI_Handler>().Ability1CDTimer(cooldown);
        if (abilityHolder2.state == AbilityHolder.AbilityState.ready)
        { GetComponent<PlayerUI_Handler>().Ability2CDTimer(activeTime); }
        yield break;
    }
    private IEnumerator AbilityCoroutine2(float cooldown, float activeTime)
    {
        if (abilityHolder1.state == AbilityHolder.AbilityState.ready)
        { GetComponent<PlayerUI_Handler>().Ability1CDTimer(activeTime); }
        GetComponent<PlayerUI_Handler>().Ability2CDTimer(cooldown);
        yield break;
    }
    #endregion

    [SerializeField] private float whirlwindSpeed = 1000;
    [HideInInspector] public bool activateWhirlwind;
    float yRotation = 0;
    public void WhirlwindAbility()
    {
        if (activateWhirlwind)
        {
            currentWeapon.SetActive(false);
            removeRotation = true;

            // Deltatime is the amount of time it takes for a frame to pass
            // This means it is an accurate way of counting upwards.
            yRotation += Time.deltaTime * whirlwindSpeed;

            // This will set the rotation to a new rotation based on an increasing Y axis.
            // Which will make it spin horizontally
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
    public void DeactivateWhirlwind()
    {
        currentWeapon.SetActive(true);
        activateWhirlwind = false;
        removeRotation = false;
        charAnimator.Play("Base Layer.IG_Idle");
    }

    public void StunPlayer(float stun)
    {
        if (!playerDead)
        {
            disabled = true;
            charAnimator.SetBool("AnimBoolStunned", true);
            Debug.Log("STUNNED");
            Invoke("Unstun", stun);
        }
    }
    public void Unstun()
    {
        if (!playerDead)
        {
            charAnimator.SetBool("AnimBoolStunned", false);
            disabled = false;
        }
    }
}
