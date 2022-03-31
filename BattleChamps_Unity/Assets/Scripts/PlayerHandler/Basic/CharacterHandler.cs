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

    public UIManager uiManager;

    [Header("Player Values")]
    public float defaultSpeed = 12f;
    public float gravity = -9.81f;
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

    [HideInInspector]
    public bool applyGravity;
    [HideInInspector]
    public bool applyMovement;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 rotationInput;

    private bool dashed = false;
    private bool attacking = false;
    private bool usingAbility = false;
    private float currentSpeed;
    private float animLength;

    private bool chargingAbility = false;
    public bool disabled = false;

    #region Customisation Variables
    public GameObject shield;

    public GameObject[] allWeapons;
    private GameObject currentWeapon;
    private WeaponHandler weaponHandler;

    //public GameObject[] headAccessories;
    //private GameObject currentHeadAccessory;

    //public GameObject[] bodyAccessories;
    //private GameObject currentBodyAccessory;
    #endregion

    public AnimationClip[] chargeAbilities;
    public AnimationClip[] quickAbilites;

    private void OnEnable()
    {
        shield.SetActive(false);

        charController = GetComponent<CharacterController>();
        charAnimator = GetComponent<Animator>();

        applyGravity = true;
        applyMovement = true;
        currentSpeed = defaultSpeed;

        uiManager = UIManager.Instance;

        foreach (GameObject weapon in allWeapons)
        {
            if (weapon.activeInHierarchy)
            {
                currentWeapon = weapon;
            }
        }

        weaponHandler = currentWeapon.GetComponent<WeaponHandler>();
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
    {
        if (!disabled)
        {
            movementInput = context.ReadValue<Vector2>();

            if (!chargingAbility && applyMovement)
            {
                float movementValue;
                float movementValueX = movementInput.x;
                float movementValueY = movementInput.y;

                if (movementValueX < 0 || movementValueY < 0)
                { movementValue = 1; }
                else if (movementValueX > 0 || movementValueY > 0)
                { movementValue = 1; }
                else
                { movementValue = 0; }

                charAnimator.SetFloat("AnimFloatRunning", movementValue);
            }
            else
            { charAnimator.SetFloat("AnimFloatRunning", 0); }
        }
    }
    public void PlayerRotation(InputAction.CallbackContext context)
    {
        if (!disabled)
        { rotationInput = context.ReadValue<Vector2>(); }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!disabled)
        {
            if (isGrounded && applyMovement)
            {
                if (!dashed)
                {
                    dashed = true;
                    StartCoroutine(DashCoroutine());
                }
            }
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!disabled)
        {
            if (!attacking && !usingAbility)
            {
                attacking = true;
                weaponHandler.EnableCollider(true);
                AnimLengthCheck();
                GetComponent<PlayerSounds>().playAttackVoiceline();
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    public void OnShield(InputAction.CallbackContext context)
    {
        if (!disabled)
        {
            if (!usingAbility)
            {
                usingAbility = true;
                AnimLengthCheck();
                StartCoroutine(ShieldCoroutine());
            }
        }
    }
    #endregion

    void Update()
    {
        applyMovement = !chargingAbility;

        if (applyGravity)
        { ApplyGravity(); }
        if (applyMovement)
        { ApplyMovement(); }
    }

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
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        charController.Move(moveDirection * currentSpeed * Time.deltaTime);

        HandleRotation();
    }
    private void HandleRotation()
    {
        Vector3 lookDirection = new Vector3(rotationInput.x, 0, rotationInput.y).normalized;

        if (rotationInput != Vector2.zero)
        { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection).normalized, 0.15F); }
    }

    private IEnumerator DashCoroutine()
    {
        currentSpeed = dashSpeed;
        yield return new WaitForSecondsRealtime(dashTime);
        currentSpeed = defaultSpeed;

        GetComponent<CharacterStats>().DashCDTimer(dashCooldown);
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
        yield return new WaitForSecondsRealtime(animLength);
        shield.SetActive(false);
        GetComponent<CharacterStats>().canBeDamaged = true;
        usingAbility = false;
    }

    private IEnumerator AbilityCoroutine()
    {
        charAnimator.SetTrigger("AnimTriggerShield");

        yield return new WaitForSecondsRealtime(animLength);
        chargingAbility = false;
        usingAbility = false;
    }
}
