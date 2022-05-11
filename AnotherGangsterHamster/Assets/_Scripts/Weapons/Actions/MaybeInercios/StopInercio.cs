using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapons.Actions
{
    public class StopInercio : WeaponAction
    {
        public float fullCharge;
        private StopStatus _currentStopStatus = StopStatus.Idle;

        private Transform stopChargeBar;

        private float _fireCharge = 0f;

        private new void Awake()
        {
            base.Awake();
            _weaponEnum = WeaponEnum.Inercio;

            stopChargeBar = GameObject.Find("StopChargeBar").transform;
        }

        public override void FireWeapon()
        {
            if (_currentStopStatus != StopStatus.Use
            && _currentStopStatus != StopStatus.Fire
            && _currentStopStatus != StopStatus.Stay)
            {
                _fireCharge = 0f;
                _currentStopStatus = StopStatus.Fire;
            }
        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {
            stopChargeBar.localScale = new Vector3(0, 1, 1);

            _currentStopStatus = StopStatus.Idle;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
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
                case StopStatus.Fire:
                    transform.position = HandPosition;

                    if(_fireCharge > fullCharge)
                    {
                        _fireCharge = fullCharge;
                    }
                    else
                        _fireCharge += Time.deltaTime;

                    stopChargeBar.localScale = new Vector3(_fireCharge / fullCharge, 1, 1);

                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        transform.position = FirePosition;
                        _myRigid.velocity = MainCameraTransform.forward * (_fireCharge / fullCharge) * fireSpeed;
                        _currentStopStatus = StopStatus.Moving;
                    }

                    break;
                case StopStatus.Moving:
                    // 움직이는 중
                    break;
            }
        }

    }
}