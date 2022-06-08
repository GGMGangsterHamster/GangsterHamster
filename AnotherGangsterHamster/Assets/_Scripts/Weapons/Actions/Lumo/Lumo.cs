using Matters.Velocity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Animation.LumoAnimation;

namespace Weapons.Actions
{
    public class Lumo : WeaponAction
    {
        public float useSpeed;

        public Vector3 GetHandPos => HandPosition;

        private LumoStatus _currentStatus = LumoStatus.Idle;
        private Transform _lumoCube;
        private LumoAnimator _lumoAnimator;

        private RaycastHit _aTypeHit;
        private Transform _aTypeTrm;
        private Vector3 _aTypeCurPos;
        private float _aTypeBeforeSize;
        private float _aTypeCurSize;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Lumo;

            _lumoCube = transform.GetComponentInChildren<LumoCube>().transform;
            _lumoCube.gameObject.SetActive(false);
            _lumoAnimator = GetComponent<LumoAnimator>();
        }

        public override void FireWeapon()
        {
            if (_currentStatus != LumoStatus.Use
            && _currentStatus != LumoStatus.Stickly)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit) && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _fireDir = MainCameraTransform.forward;

                    _aTypeHit = hit;
                    _aTypeTrm = hit.transform;
                    _aTypeCurPos = hit.transform.position - hit.point;
                    _aTypeCurSize = _aTypeHit.transform.localScale.x;
                    _currentStatus = LumoStatus.Stickly;

                    _lumoAnimator.FireAnime(transform.position, hit.point, fireSpeed, Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0));
                }
            }
        }

        public override void UseWeapon()
        {
            if (_currentStatus == LumoStatus.Stickly && _lumoAnimator.isStopedMoving())
            {
                _currentStatus = LumoStatus.Use;
                _lumoCube.rotation = Quaternion.LookRotation(_aTypeHit.normal) * Quaternion.Euler(90, 0, 0);
                _lumoAnimator.UsingAnime(_aTypeHit.normal, useSpeed);
            }
        }

        public override void ResetWeapon()
        {
            if (_currentStatus == LumoStatus.Reset) return;

            PlayerBaseTransform.GetComponent<FollowGroundPos>().Deactive(_lumoCube.gameObject);

            _lumoCube.gameObject.SetActive(false);

            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _aTypeTrm = null;
            _currentStatus = LumoStatus.Reset;
            _lumoAnimator.ResetAnime(transform.position, HandPosition, fireSpeed, Quaternion.LookRotation((transform.position - MainCameraTransform.position).normalized) * Quaternion.Euler(90, 0, 0));
        }

        public override bool IsHandleWeapon()
        {
            return _currentStatus == LumoStatus.Idle;
        }

        public override Transform SticklyTrm()
        {
            return _aTypeTrm;
        }

        private void Update()
        {
            switch(_currentStatus)
            {
                case LumoStatus.Stickly:
                    SettingLumoPos();
                    break;
                case LumoStatus.Use:
                    if(_lumoAnimator.isStopedMoving() && !_lumoCube.gameObject.activeSelf)
                        _lumoCube.gameObject.SetActive(true);
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