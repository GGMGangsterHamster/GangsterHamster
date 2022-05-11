using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class ReinforcementInercio : WeaponAction
    {
        private ReinforcementStatus _currrentStatus = ReinforcementStatus.Idle;
        private Transform _reinforcementCube;
        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Inercio;

            _reinforcementCube = transform.GetChild(0);
        }

        //Idle,
        //Fire,
        //Use,
        //Stickly

        public override void FireWeapon()
        {
            if(_currrentStatus == ReinforcementStatus.Idle)
            {
                _myRigid.constraints = RigidbodyConstraints.None;
                _fireDir = MainCameraTransform.forward;
                _currrentStatus = ReinforcementStatus.Fire;
                (_myCollider as SphereCollider).center = Vector3.zero;
            }
        }

        public override void UseWeapon()
        {
            if(_currrentStatus != ReinforcementStatus.Use)
            {
                transform.rotation = Quaternion.identity;
                _myRigid.constraints = RigidbodyConstraints.FreezeAll;
                _reinforcementCube.gameObject.SetActive(true);
                _currrentStatus = ReinforcementStatus.Use;
            }
        }

        public override void ResetWeapon()
        {
            _currrentStatus = ReinforcementStatus.Idle;
            _reinforcementCube.gameObject.SetActive(false);
        }

        public override bool IsHandleWeapon()
        {
            return _currrentStatus == ReinforcementStatus.Idle;
        }

        private void Update()
        {
            switch(_currrentStatus)
            {
                case ReinforcementStatus.Idle:
                    (_myCollider as SphereCollider).center = Vector3.one * short.MaxValue;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;
                    transform.position = HandPosition;
                    break;
                case ReinforcementStatus.Fire:
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
            }
        }
    }
}