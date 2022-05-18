using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class ReinforcementInercio : WeaponAction
    {
        private ReinforcementStatus _currentStatus = ReinforcementStatus.Idle;
        private Transform _reinforcementCapsule;
        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Inercio;

            _reinforcementCapsule = transform.GetChild(0);
        }

        public override void FireWeapon()
        {
            if (_currentStatus != ReinforcementStatus.Use
            && _currentStatus != ReinforcementStatus.Stickly)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit) && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _fireDir = MainCameraTransform.forward;
                    transform.position = hit.point + (hit.normal * transform.localScale.y / 2);
                    transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);

                    _currentStatus = ReinforcementStatus.Stickly;
                }
            }
        }

        public override void UseWeapon()
        {
            if (_currentStatus == ReinforcementStatus.Stickly)
            {
                _reinforcementCapsule.gameObject.SetActive(true);
            }
        }

        public override void ResetWeapon()
        {
            _currentStatus = ReinforcementStatus.Idle;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;

            _reinforcementCapsule.gameObject.SetActive(false);
        }

        public override bool IsHandleWeapon()
        {
            return _currentStatus == ReinforcementStatus.Idle;
        }

        private void Update()
        {
            switch(_currentStatus)
            {
                case ReinforcementStatus.Idle:
                    transform.position = HandPosition;
                    break;
            }
        }
    }
}