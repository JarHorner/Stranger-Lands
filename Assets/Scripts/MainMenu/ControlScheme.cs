using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlScheme
{
    #region Variables 
    public static bool isController { get; private set; }
    public static Keyboard keyboardControlScheme { get; set; }
    public static Gamepad gamepadControlScheme { get; set; }
    #endregion

    #region Methods

    // used in the start methods of PlayerController and MainMenu, ensure the static state is populated.
    public static void GetControlSchemes()
    {
        keyboardControlScheme = Keyboard.current;
        gamepadControlScheme = Gamepad.current;
    }

    // used in the Update methods of PlayerController and MainMenu to monitor every frame which input device is being used.
    public static void GetUsedControlScheme()
    {
        if (keyboardControlScheme.anyKey.wasPressedThisFrame)
        {
            isController = false;
        }
        else if (gamepadControlScheme != null)
        {
            if (gamepadControlScheme.aButton.wasPressedThisFrame || gamepadControlScheme.bButton.wasPressedThisFrame 
                || gamepadControlScheme.xButton.wasPressedThisFrame || gamepadControlScheme.yButton.wasPressedThisFrame || gamepadControlScheme.rightTrigger.wasPressedThisFrame 
                || gamepadControlScheme.selectButton.wasPressedThisFrame || gamepadControlScheme.startButton.wasPressedThisFrame || gamepadControlScheme.leftStick.down.wasPressedThisFrame 
                || gamepadControlScheme.leftStick.right.wasPressedThisFrame || gamepadControlScheme.leftStick.left.wasPressedThisFrame || gamepadControlScheme.leftStick.up.wasPressedThisFrame)
            {
                isController = true;
            }
        }
    }

    #endregion
}
