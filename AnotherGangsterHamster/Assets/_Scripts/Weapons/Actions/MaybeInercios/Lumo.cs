using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Lumo : WeaponAction
    {
        private LumoStatus _currentStatus = LumoStatus.Idle;
        private Transform _lumoCube;

        private RaycastHit _aTypeHit;
        private Transform _aTypeTrm;
        private Vector3 _aTypeCurPos;
        private float _aTypeBeforeSize;
        private float _aTypeCurSize;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Lumo;

            _lumoCube = transform.GetChild(0);
        }

        public override void FireWeapon()
        {
            if (_currentStatus != LumoStatus.Use
            && _currentStatus != LumoStatus.Stickly)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit) && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _fireDir = MainCameraTransform.forward;
                    transform.position = hit.point + (hit.normal * transform.localScale.y / 2);
                    transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);

                    _aTypeHit = hit;
                    _aTypeTrm = hit.transform;
                    _aTypeCurPos = hit.transform.position - hit.point;
                    _aTypeCurSize = _aTypeHit.transform.localScale.x;

                    _currentStatus = LumoStatus.Stickly;
                }
            }
        }

        public override void UseWeapon()
        {
            if (_currentStatus == LumoStatus.Stickly)
            {
                _lumoCube.gameObject.SetActive(true);
            }
        }

        public override void ResetWeapon()
        {
            _lumoCube.gameObject.SetActive(false);

            _currentStatus = LumoStatus.Idle;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        public override bool IsHandleWeapon()
        {
            return _currentStatus == LumoStatus.Idle;
        }

        private void Update()
        {
            switch(_currentStatus)
            {

                case LumoStatus.Idle:
                    transform.position = HandPosition;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MainCameraTransform.forward), 0.5f);
                    break;
                case LumoStatus.Stickly:
                    SettingLumoPos();
                    break;
            }
        }
        private void SettingLumoPos()
        {
            if (_aTypeCurPos != _aTypeTrm.position - _aTypeHit.point)
            {
                transform.position -= _aTypeCurPos - (_aTypeTrm.position - _aTypeHit.point);
                _aTypeCurPos = _aTypeTrm.position - _aTypeHit.point;
            }
            if (_aTypeCurSize != _aTypeHit.transform.localScale.x)
            {
                _aTypeBeforeSize = _aTypeCurSize;
                _aTypeCurSize = _aTypeHit.transform.localScale.x;
                transform.position -= _aTypeHit.normal * (_aTypeBeforeSize - _aTypeCurSize) / 2;
            }
        }
    }
}