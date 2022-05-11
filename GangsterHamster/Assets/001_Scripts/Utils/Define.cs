using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    private static Transform _playerBaseTrm;
    public static Transform PlayerBaseTrm
    {
        get
        {
            if(_playerBaseTrm == null)
            {
                _playerBaseTrm = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
            }

            return _playerBaseTrm;
        }
    }

    private static Transform _playerTrm;
    public static Transform PlayerTrm
    {
        get
        {
            if(_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("PLAYER").transform;
            }

            return _playerTrm;
        }
    }

    private static Transform _rightHandTrm;
    public static Transform RightHandTrm
    {
        get
        {
            if(_rightHandTrm == null)
            {
                _rightHandTrm = GameObject.Find("RightHand").transform;
            }

            return _rightHandTrm;
        }
    }

    private static Transform _mainCamTrm;
    public static Transform MainCamTrm
    {
        get
        {
            if(_mainCamTrm == null)
            {
                _mainCamTrm = Camera.main.transform;
            }
            return _mainCamTrm;
        }
    }
}
