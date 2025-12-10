using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static Vector2 PointerPosition { get; private set; }
    public static bool TapThisFrame { get; private set; }
    public static bool CancelThisFrame { get; private set; }

    void Awake()
    {
        if (Touchscreen.current != null)
        {
            InputSystem.EnableDevice(Touchscreen.current);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        TapThisFrame = false;
        CancelThisFrame = false;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                PointerPosition = touch.position.ReadValue();
                TapThisFrame = true;
                return;
            }
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerPosition = Mouse.current.position.ReadValue();
            TapThisFrame = true;
            return;
        }

        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            CancelThisFrame = true;
        }
    }
}
