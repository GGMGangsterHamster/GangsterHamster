using Characters.Player;
using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class ReinforcementCube : MonoBehaviour
    {
        private bool _isReinforcemented = false;
        private GravityAffectedObject _playerGravity;

        private void Awake()
        {
            _playerGravity = GameObject.FindGameObjectWithTag("PLAYER_BASE").GetComponent<GravityAffectedObject>();
        }

        public void ObjTriggerStayEvent(GameObject obj)
        {
            Debug.Log("Stay");
            if (!_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = false;
                _playerGravity.SetIndividualGravity(GravityManager.GetGlobalGravityDirection(), 4.9f);
                _isReinforcemented = true;
            }

            if (PlayerStatus.IsRunning)
            {
                PlayerValues.Speed = PlayerValues.DashSpeed * 2;
            }
            else
            {
                PlayerValues.Speed = PlayerValues.WalkingSpeed * 2;
            }
        }

        public void ObjTriggerExitEvent(GameObject obj)
        {
            Debug.Log("Exited");
            if (_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = true;
                _isReinforcemented = false;
            }
        }

        private void OnDisable()
        {
            _playerGravity.AffectedByGlobalGravity = true;
            _isReinforcemented = false;
        }
    }
}