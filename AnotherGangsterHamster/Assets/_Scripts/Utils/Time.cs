using Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public partial class Utils
{
    private static float _mouseSpeed = 0;
    static public void StopTime()
    {
        _mouseSpeed = PlayerValues.MouseSpeed;
        PlayerValues.MouseSpeed = 0;
        Time.timeScale = 0;
    }

    static public void MoveTime()
    {
        if(_mouseSpeed != 0)
        {
            PlayerValues.MouseSpeed = _mouseSpeed;
        }
        Time.timeScale = 1;
    }
}
