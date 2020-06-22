using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controll 
{
     public static int horizontal_move;
    public static bool up_move;
    public static bool start_up_move;
    public static int jumpLong;
    public static bool down_move;

    public static float GetHorizontalMove()
    {
        //return Input.GetAxis("Horizontal");
        return horizontal_move;
    }
    public static bool GetJumpStart()
    {
        //return Input.GetKeyDown(KeyCode.Space);
        return up_move;
    }
    public static bool GetJumpHold()
    {
        //return Input.GetKey(KeyCode.Space);
        return up_move;
    }
    public static bool GetJumpStop()
    {
        //return Input.GetKeyUp(KeyCode.Space);
        return false;
    }
    public static bool GetDown()
    {
        //return Input.GetKey(KeyCode.S);
        return down_move;
    }


    static void Start()
    {
        KinectManager.Left += new KinectManager.SimpleEvent(Left);
        KinectManager.Right += new KinectManager.SimpleEvent(Right);
        KinectManager.Up += new KinectManager.SimpleEvent(Up);
        KinectManager.Down += new KinectManager.SimpleEvent(Down);
        KinectManager.Stop += new KinectManager.SimpleEvent(Stop);
    }

    static void Left()
    {
        if (horizontal_move == 0) horizontal_move = -1;
        Debug.Log(horizontal_move);
    }
    static void Right()
    {
        if (horizontal_move == 0) horizontal_move = 1;
    }
    static void Down()
    {
        down_move = true;
    }

    static void Up()
    {
        up_move = true;
    }
    static void Stop()
    {
        horizontal_move = 0;
    }
    
    
}
