using Characters.Player;
using Matters.Gravity;
using Objects.InteractableObjects;
using UnityEngine;
using Weapons.Actions.Effect;

namespace Weapons.Actions
{
    public class LumoCube : MonoBehaviour
    {
        private bool _isReinforcemented = false;
        private GravityAffectedObject _playerGravity;
        private Rigidbody _playerRigid;
        private LumoCubeEffect effect;
        private InteractableObjects interactableObj;

        private void Awake()
        {
            _playerGravity = GameObject.FindGameObjectWithTag("PLAYER_BASE").GetComponent<GravityAffectedObject>();
            _playerRigid = _playerGravity?.GetComponent<Rigidbody>();
            effect = GetComponent<LumoCubeEffect>();
            interactableObj = GetComponent<InteractableObjects>();
        }

        public void ObjTriggerStayEvent(GameObject obj)
        {
            if (!obj.CompareTag("PLAYER_BASE")) return;

            if (!_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = false;
                _playerGravity.SetIndividualGravity(GravityManager.GetGlobalGravityDirection(), 4.9f);
                if(_playerRigid != null)
                    _playerRigid.mass = 2;
                _isReinforcemented = true;
                effect.EffectOn();
            }
            else if(_playerGravity.AffectedByGlobalGravity)
            {
                _playerGravity.AffectedByGlobalGravity = false;
                _playerGravity.SetIndividualGravity(GravityManager.GetGlobalGravityDirection(), 4.9f);
            }

            PlayerValues.Speed = PlayerValues.DashSpeed * 2;
        }

        public void ObjTriggerExitEvent(GameObject obj)
        {
            if (!obj.CompareTag("PLAYER_BASE")) return;

            if (_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = true;
                _isReinforcemented = false;
                if (_playerRigid != null)
                    _playerRigid.mass = 1;

                if (PlayerStatus.OnGround)
                {
                    PlayerValues.Speed = PlayerValues.DashSpeed;
                }

                effect.EffectOff();
            }
        }

        private void OnDisable()
        {
            _playerGravity.AffectedByGlobalGravity = true;
            _isReinforcemented = false;
            if (_playerRigid != null)
                _playerRigid.mass = 1;

            if(PlayerStatus.OnGround)
            {
                PlayerValues.Speed = PlayerValues.DashSpeed;
            }
            
            effect.EffectOff();
            interactableObj.ForceEventExit(_playerGravity.gameObject);
        }
    }
}