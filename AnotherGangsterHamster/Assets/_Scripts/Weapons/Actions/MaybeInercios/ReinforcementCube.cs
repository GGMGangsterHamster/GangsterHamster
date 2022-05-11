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
                PlayerValues.Speed = PlayerValues.Speed * 2;
                _isReinforcemented = true;
            }
            else
            {

            }
        }

        public void ObjTriggerExitEvent(GameObject obj)
        {
            Debug.Log("되긴 하는걸까");
            if (_isReinforcemented)
            {
                Debug.Log("Exited");
                _playerGravity.AffectedByGlobalGravity = true;
                PlayerValues.Speed = PlayerValues.WalkingSpeed;
                _isReinforcemented = false;
            }
        }

    }
}