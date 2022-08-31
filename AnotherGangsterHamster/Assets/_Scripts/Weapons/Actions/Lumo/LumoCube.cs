using Characters.Player;
using Matters.Gravity;
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

        private void Awake()
        {
            _playerGravity = GameObject.FindGameObjectWithTag("PLAYER_BASE").GetComponent<GravityAffectedObject>();
            _playerRigid = _playerGravity.GetComponent<Rigidbody>();
            effect = GetComponent<LumoCubeEffect>();
        }

        public void ObjTriggerStayEvent(GameObject obj)
        {
            if (!obj.CompareTag("PLAYER_BASE")) return;

            if (!_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = false;
                _playerGravity.SetIndividualGravity(GravityManager.GetGlobalGravityDirection(), 4.9f);
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
            Debug.Log("나간거니?");
            if (_isReinforcemented)
            {
                _playerGravity.AffectedByGlobalGravity = true;
                _isReinforcemented = false;
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
            _playerRigid.mass = 1;

            if(PlayerStatus.OnGround)
            {
                PlayerValues.Speed = PlayerValues.DashSpeed;
            }
            
            effect.EffectOff();
        }
    }
}