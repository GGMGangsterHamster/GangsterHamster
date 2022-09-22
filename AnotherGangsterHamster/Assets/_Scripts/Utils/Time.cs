using Characters.Player;
using Setting.VO;
using System.Collections;
using System.Collections.Generic;
using UI.PanelScripts;
using UnityEngine;

static public partial class Utils
{
    private static string _mousePath = "SettingValue/Sensitivity.json";
    static public void StopTime()
    {
        PlayerValues.CanMouseMove = false;
        Time.timeScale = 0;
    }

    static public void MoveTime()
    {
        MouseVO mouseVO = JsonToVO<MouseVO>(_mousePath);

        if (mouseVO != null)
            UIManager.Instance.sensitivityAction(mouseVO.sensitivity);
        else
        {
            PlayerValues.MouseSpeed = 2f;
        }

        PlayerValues.CanMouseMove = true;
        Time.timeScale = 1;
    }
}
