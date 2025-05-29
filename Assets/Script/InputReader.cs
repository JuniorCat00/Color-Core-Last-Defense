using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static Vector2 PointerPosition { get; private set; }
    public static bool TapThisFrame { get; private set; }
    public static bool CancelThisFrame { get; private set; }

    private void Update()
    {
        TapThisFrame = false;
        CancelThisFrame = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            PointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            TapThisFrame = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerPosition = Mouse.current.position.ReadValue();
            TapThisFrame = true;
        }

        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CancelThisFrame = true;
        }
    }
}
