using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Inercio : WeaponAction
    {
        private InercioStatus _currentInercioStatus = InercioStatus.Idle;

        private GameObject sticklyObj;
        private Rigidbody sticklyObjRigid;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Lumo;
        }

        #region Actions
        public override void FireWeapon()
        {
            if (_currentInercioStatus != InercioStatus.Idle) return;

            transform.position = FirePosition;
            _fireDir = MainCameraTransform.forward;
            _myRigid.constraints = RigidbodyConstraints.None;
            _myRigid.velocity = _fireDir * fireSpeed;
            _myCollider.isTrigger = false;

            _currentInercioStatus = InercioStatus.Fire;
        }

        public override void UseWeapon()
        {
            if(sticklyObj != null)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, sticklyObj.transform.lossyScale.x))
                {
                    sticklyObj.transform.position = FirePosition;
                }
                else
                {
                    sticklyObj.transform.position = FirePosition + (sticklyObj.transform.lossyScale.x * MainCameraTransform.forward);
                }

                sticklyObjRigid.constraints = RigidbodyConstraints.None;
                sticklyObj = null;

                ResetWeapon();
            }
        }

        public override void ResetWeapon()
        {
            Stop();

            _currentInercioStatus = InercioStatus.Idle;
            _myCollider.isTrigger = true;

            if(sticklyObj != null)
            {
                sticklyObj = null;


                sticklyObjRigid.constraints = RigidbodyConstraints.None;
            }
        }

        public override bool IsHandleWeapon()
        {
            return _currentInercioStatus == InercioStatus.Idle;
        }
        #endregion

        #region CollisionEvents
        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {
            Stop();

            sticklyObj = obj;
            sticklyObjRigid = obj.GetComponent<Rigidbody>();

            sticklyObjRigid.constraints = RigidbodyConstraints.FreezeAll;
        }
        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            _myRigid.useGravity = true;
            _currentInercioStatus = InercioStatus.LosePower;
        }
        #endregion

        private void Update()
        {
            switch(_currentInercioStatus)
            {
                case InercioStatus.Idle:
                    transform.position = HandPosition;
                    break;
                case InercioStatus.Fire:

                    break;
                case InercioStatus.LosePower:
                    break;
            }
        }

        private void Stop()
        {
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.useGravity = false;
        }
    }
}