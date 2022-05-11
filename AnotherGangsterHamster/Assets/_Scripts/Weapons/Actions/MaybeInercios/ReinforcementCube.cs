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
            if (!_isReinforcemented)
            {
                Debug.Log("Stay..");
                _playerGravity.AffectedByGlobalGravity = false;
                _playerGravity.SetIndividualGravity(Vector3.down, 4.9f);
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
            if (_isReinforcemented)
            {
                Debug.Log("Exited");
                _playerGravity.AffectedByGlobalGravity = true;
                _isReinforcemented = false;
            }
        }

    }
}