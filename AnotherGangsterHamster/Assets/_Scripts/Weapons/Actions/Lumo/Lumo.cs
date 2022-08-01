using Matters.Velocity;
using Objects.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;
using Weapons.Animation;

namespace Weapons.Actions
{
    public class Lumo : WeaponAction
    {
        public float useSpeed;

        public Vector3 GetHandPos => HandPosition;

        private LumoStatus _currentStatus = LumoStatus.Idle;
        private Transform _lumoCube;
        private LumoAnimator _lumoAnimator;

        private AllWeaponMessageBroker
            _AWmessageBroker;
        private LumoMessageBroker _messageBroker;

        private Grand _grand;

        private RaycastHit _aTypeHit;
        private Transform _aTypeTrm;
        private Vector3 _aTypeCurPos;
        private float _aTypeBeforeSize;
        private float _aTypeCurSize;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Lumo;

            _grand = FindObjectOfType<Grand>();
            _lumoCube = transform.GetComponentInChildren<LumoCube>().transform;
            _lumoCube.gameObject.SetActive(false);
            _lumoAnimator = GetComponent<LumoAnimator>();

            _messageBroker = GetComponent<LumoMessageBroker>();
            _AWmessageBroker = transform.parent.GetComponent<AllWeaponMessageBroker>();
        }

        public override void FireWeapon()
        {
            if (_currentStatus != LumoStatus.Use
            && _currentStatus != LumoStatus.Stickly
            && _lumoAnimator.isStopedMoving())
            { 
                if (InteractionManager.Instance.currentRaycastHitTrm != null
                    && InteractionManager.Instance.currentRaycastHitTrm.CompareTag("ATYPEOBJECT"))
                {
                    _AWmessageBroker.OnFire?.Invoke();
                    _messageBroker.OnFire?.Invoke();

                    RaycastHit hit = InteractionManager.Instance.currentRaycastHit;
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
                _AWmessageBroker.OnUse?.Invoke();
                _messageBroker.OnUse?.Invoke();

                _currentStatus = LumoStatus.Use;
                _lumoCube.rotation = Quaternion.LookRotation(_aTypeHit.normal) * Quaternion.Euler(90, 0, 0);
                _lumoAnimator.UsingAnime(_aTypeHit.normal, useSpeed);
            }
        }

        public override void ResetWeapon()
        {
            if (_currentStatus == LumoStatus.Idle || !_lumoAnimator.isStopedMoving())
            {
                return;
            }

            _AWmessageBroker.OnReset?.Invoke();
            _messageBroker.OnReset?.Invoke();
            _lumoAnimator.RotationReset();
            PlayerBaseTransform.GetComponent<FollowGroundPos>().Deactive(_lumoCube.gameObject);
            _currentStatus = LumoStatus.Idle;
            _lumoCube.gameObject.SetActive(false);

            if (!gameObject.activeSelf)
            {
                transform.position = HandPosition;
                _lumoAnimator.PositionReset(HandPosition);
                return;
            }

            _aTypeTrm = null;
            _lumoAnimator.ResetAnime(transform.position, HandPosition, fireSpeed);
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
                    {
                        _lumoCube.gameObject.SetActive(true);
                    }
                        
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
            if (_aTypeCurSize != _aTypeHit.transform.localScale.x * _grand._sizeLevelValue[_grand._beforeSizeLevel])
            {
                _aTypeBeforeSize = _aTypeCurSize;
                _aTypeCurSize = _aTypeHit.transform.localScale.x * _grand._sizeLevelValue[_grand._currentSizeLevel];
                transform.position -= _aTypeHit.normal * (_aTypeBeforeSize - _aTypeCurSize) / 2;
            }
        }
    }
}