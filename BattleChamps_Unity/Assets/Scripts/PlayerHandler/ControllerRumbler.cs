using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class ControllerRumbler : MonoBehaviour
{
    PlayerInput playerInput;

    public void InitializeRumbler()
    {
        playerInput = GetComponent<CharacterHandler>().playerConfig.Input;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRumble(float duration, int playerID)
    {
        Gamepad gamepad = (Gamepad)playerInput.devices[0];
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.25f, 0.75f);
            Invoke("StopRumble", duration);
        }
    }

    public void StopRumble()
    {
        Gamepad gamepad = (Gamepad)playerInput.devices[0];
        gamepad.SetMotorSpeeds(0, 0);
    }
}
