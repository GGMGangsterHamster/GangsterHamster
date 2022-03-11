using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    private static Transform _playerTrm;
    public static Transform PlayerTrm
    {
        get
        {
            if(_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
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
}
