using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static float GetHorzAxis()
    {
        return Mathf.Round(Input.GetAxisRaw("Horizontal"));
    }

    public static float GetVertAxis()
    {
        return Mathf.Round(Input.GetAxisRaw("Vertical"));
    }

    public static Vector2 GetMovement()
    {
        return new Vector2(GetHorzAxis(), GetVertAxis());
    }

    public static bool GetJump()
    {
        return Input.GetAxisRaw("Jump") != 0f;
    }

    public static bool GetFireA()
    {
        return Input.GetAxisRaw("Fire1") != 0f;
    }

    public static bool GetFireB()
    {
        return Input.GetAxisRaw("Fire2") != 0f;
    }
    public static bool GetFireC()
    {
        return Input.GetAxisRaw("Fire3") != 0f;
    }
}
