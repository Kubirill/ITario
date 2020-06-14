using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controll 
{
    public static float GetHorizontalMove()
    {
        return Input.GetAxis("Horizontal");
    }
    public static bool GetJumpStart()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    public static bool GetJumpHold()
    {
        return Input.GetKey(KeyCode.Space);
    }
    public static bool GetJumpStop()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }
    public static bool GetDown()
    {
        return Input.GetKeyDown(KeyCode.S);
    }
}
