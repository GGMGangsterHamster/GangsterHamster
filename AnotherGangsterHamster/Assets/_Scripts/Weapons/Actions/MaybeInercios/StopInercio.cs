using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapons.Actions
{
    public class StopInercio : WeaponAction
    {
        private StopStatus _currentStopStatus = StopStatus.Idle;
        private Transform stopPillarTrm;

        private new void Awake()
        {
            base.Awake();
            _weaponEnum = WeaponEnum.Inercio;

            stopPillarTrm = transform.GetChild(0);
        }

        public override void FireWeapon()
        {
            if (_currentStopStatus != StopStatus.Use
            && _currentStopStatus != StopStatus.Stickly)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit))
                {
                    _fireDir = MainCameraTransform.forward;
                    transform.position = hit.point + (hit.normal * transform.localScale.y / 2);
                    transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
                    
                    _currentStopStatus = StopStatus.Stickly;
                }
            }
        }

        public override void UseWeapon()
        {
            if(_currentStopStatus == StopStatus.Stickly)
            {
                stopPillarTrm.gameObject.SetActive(true);
            }
            else
            {
                // 이 경우는 플레이어 정지
            }
        }

        public override void ResetWeapon()
        {
            _currentStopStatus = StopStatus.Idle;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;

            stopPillarTrm.gameObject.SetActive(false);
        }

        public override bool IsHandleWeapon()
        {
            return _currentStopStatus == StopStatus.Idle;
        }

        private void Update()
        {
            switch (_currentStopStatus)
            {
                case StopStatus.Idle:
                    transform.position = HandPosition;
                    break;
            }
        }

    }
}