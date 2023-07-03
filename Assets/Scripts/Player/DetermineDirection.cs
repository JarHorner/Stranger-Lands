using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DetermineDirection
{
    #region Variables
    private static string firstKey = "";
    private static string firstAnalogDirection = "";

    #endregion

    #region Methods

    public static int[] DetermineKeyboardFacingDirection(Animator animator, Vector2 movement, int movingHorizontal, int movingVertical)
    {
        var keyboard = ControlScheme.keyboardControlScheme;
        CheckFirstKey(firstKey, keyboard);
        if (firstKey == "")
        {
            if (keyboard.upArrowKey.isPressed)
            {
                movingVertical = 1;
                movingHorizontal = 0;
                firstKey = "up";
            }
            else if (keyboard.downArrowKey.isPressed)
            {
                movingVertical = -1;
                movingHorizontal = 0;
                firstKey = "down";
            }
            else if (keyboard.leftArrowKey.isPressed)
            {
                movingHorizontal = -1;
                movingVertical = 0;
                firstKey = "left";
            }
            else if (keyboard.rightArrowKey.isPressed)
            {
                movingHorizontal = 1;
                movingVertical = 0;
                firstKey = "right";
            }
        }
        else
        {
            if ((keyboard.rightArrowKey.isPressed && firstKey == "up") || (keyboard.leftArrowKey.isPressed && firstKey == "up"))
            {
                movingVertical = 1;
                movingHorizontal = 0;
            }
            else if ((keyboard.rightArrowKey.isPressed && firstKey == "down") || (keyboard.leftArrowKey.isPressed && firstKey == "down"))
            {
                movingVertical = -1;
                movingHorizontal = 0;
            }
            else if ((keyboard.upArrowKey.isPressed && firstKey == "left") || (keyboard.downArrowKey.isPressed && firstKey == "left"))
            {
                movingHorizontal = -1;
                movingVertical = 0;
            }
            else if ((keyboard.upArrowKey.isPressed && firstKey == "right") || (keyboard.downArrowKey.isPressed && firstKey == "right"))
            {
                movingHorizontal = 1;
                movingVertical = 0;
            }
        }
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        int[] movingAxis = new int[] {movingHorizontal, movingVertical};
        return movingAxis;
    }

    private static void CheckFirstKey(string direction, Keyboard keyboard)
    {
        bool keyStillPressed = false;
        switch (direction)
        {
            case "up":
                keyStillPressed = keyboard.upArrowKey.isPressed;
                break;
            case "down":
                keyStillPressed = keyboard.downArrowKey.isPressed;
                break;
            case "left":
                keyStillPressed = keyboard.leftArrowKey.isPressed;
                break;
            case "right":
                keyStillPressed =  keyboard.rightArrowKey.isPressed;
                break;
        }
        if (!keyStillPressed)
        {
            firstKey = "";
        }
    }

    public static int[] DetermineGamepadFacingDirection(Animator animator, Vector2 movement, int movingHorizontal, int movingVertical)
    {
        var gamepad = ControlScheme.gamepadControlScheme;
        CheckFirstAnalogDirection(firstAnalogDirection, gamepad);
        if (firstAnalogDirection == "")
        {
            if (gamepad.leftStick.up.isPressed)
            {
                movingVertical = 1;
                movingHorizontal = 0;
                firstAnalogDirection = "up";
            }
            else if (gamepad.leftStick.down.isPressed)
            {
                movingVertical = -1;
                movingHorizontal = 0;
                firstAnalogDirection = "down";
            }
            else if (gamepad.leftStick.left.isPressed)
            {
                movingHorizontal = -1;
                movingVertical = 0;
                firstAnalogDirection = "left";
            }
            else if (gamepad.leftStick.right.isPressed)
            {
                movingHorizontal = 1;
                movingVertical = 0;
                firstAnalogDirection = "right";
            }
        }
        else
        {
            if ((gamepad.leftStick.right.isPressed && firstAnalogDirection == "up") || (gamepad.leftStick.left.isPressed && firstAnalogDirection == "up"))
            {
                movingVertical = 1;
                movingHorizontal = 0;
            }
            else if ((gamepad.leftStick.right.isPressed && firstAnalogDirection == "down") || (gamepad.leftStick.left.isPressed && firstAnalogDirection == "down"))
            {
                movingVertical = -1;
                movingHorizontal = 0;
            }
            else if ((gamepad.leftStick.up.isPressed && firstAnalogDirection == "left") || (gamepad.leftStick.down.isPressed && firstAnalogDirection == "left"))
            {
                movingHorizontal = -1;
                movingVertical = 0;
            }
            else if ((gamepad.leftStick.up.isPressed && firstKey == "right") || (gamepad.leftStick.down.isPressed && firstKey == "right"))
            {
                movingHorizontal = 1;
                movingVertical = 0;
            }
        }
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        int[] movingAxis = new int[] {movingHorizontal, movingVertical};
        return movingAxis;
    }

    private static void CheckFirstAnalogDirection(string direction, Gamepad gamepad)
    {
        bool analogDirectionStillPressed = false;
        switch (direction)
        {
            case "up":
                analogDirectionStillPressed = gamepad.leftStick.up.isPressed;
                break;
            case "down":
                analogDirectionStillPressed = gamepad.leftStick.down.isPressed;
                break;
            case "left":
                analogDirectionStillPressed = gamepad.leftStick.left.isPressed;
                break;
            case "right":
                analogDirectionStillPressed =  gamepad.leftStick.right.isPressed;
                break;
        }
        if (!analogDirectionStillPressed)
        {
            firstAnalogDirection = "";
        }
    }

    #endregion
}
