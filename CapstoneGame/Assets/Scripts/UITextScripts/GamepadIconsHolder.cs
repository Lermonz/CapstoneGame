using System;
using UnityEngine;

public class GamepadIconsHolder : MonoBehaviour
{
    public GamepadIcons _icons;
}
[Serializable]
public struct GamepadIcons
{
    public Sprite buttonSouth;
    public Sprite buttonNorth;
    public Sprite buttonEast;
    public Sprite buttonWest;
    public Sprite startButton;
    public Sprite selectButton;
    public Sprite leftTrigger;
    public Sprite rightTrigger;
    public Sprite leftShoulder;
    public Sprite rightShoulder;
    public Sprite dpadUp;
    public Sprite dpadDown;
    public Sprite dpadLeft;
    public Sprite dpadRight;
    public Sprite leftStickUp;
    public Sprite leftStickDown;
    public Sprite leftStickLeft;
    public Sprite leftStickRight;
    public Sprite rightStickUp;
    public Sprite rightStickDown;
    public Sprite rightStickLeft;
    public Sprite rightStickRight;

    public Sprite GetSprite(string controlPath)
    {
        // From the input system, we get the path of the control on device. So we can just
        // map from that to the sprites we have for gamepads.
        switch (controlPath)
        {
            case "buttonSouth": return buttonSouth;
            case "buttonNorth": return buttonNorth;
            case "buttonEast": return buttonEast;
            case "buttonWest": return buttonWest;
            case "start": return startButton;
            case "select": return selectButton;
            case "leftTrigger": return leftTrigger;
            case "rightTrigger": return rightTrigger;
            case "leftShoulder": return leftShoulder;
            case "rightShoulder": return rightShoulder;
            case "dpad/up": return dpadUp;
            case "dpad/down": return dpadDown;
            case "dpad/left": return dpadLeft;
            case "dpad/right": return dpadRight;
            case "leftStick/up": return leftStickUp;
            case "leftStick/down": return leftStickDown;
            case "leftStick/left": return leftStickLeft;
            case "leftStick/right": return leftStickRight;
            case "rightStick/up": return rightStickUp;
            case "rightStick/down": return rightStickDown;
            case "rightStick/left": return rightStickLeft;
            case "rightStick/right": return rightStickRight;
        }
        return null;
    }
}
