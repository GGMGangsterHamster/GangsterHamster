using Characters.Player;
using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class PlayerResetValues : MonoBehaviour, ICallbackable
    {
        private GravityAffectedObject _playerGravity;

        private void Awake()
        {
            _playerGravity = GameObject.FindGameObjectWithTag("PLAYER_BASE").GetComponent<GravityAffectedObject>();
        }

        public void Invoke(object param)
        {
            if (_playerGravity.AffectedByGlobalGravity)
            {
                if(PlayerStatus.IsRunning)
                {
                    PlayerValues.Speed = PlayerValues.DashSpeed;
                }
                else
                {
                    PlayerValues.Speed = PlayerValues.WalkingSpeed;
                }
            }
        }
    }
}
