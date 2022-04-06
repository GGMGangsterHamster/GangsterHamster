using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Inercio : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public float FireSpeed;

        // ���Ⱑ ���� �� �ִ� ��Ȳ�� ��Ƴ���
        private enum InercioStatus
        {
            Idle,
            Fire,
            Use,
            Stickly,
            Reset,
        }

        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTrasnform;

        #region Propertitys
        private Transform MainCameraTransform
        {
            get
            {
                if(_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }
        private Transform PlayerBaseTransform
        {
            get
            {
                if(_playerBaseTransform == null)
                {
                    _playerBaseTransform = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
                }

                return _playerBaseTransform;
            }
        }
        private Transform PlayerTrasnform
        {
            get
            {
                if (_playerTrasnform == null)
                {
                    _playerTrasnform = GameObject.FindGameObjectWithTag("PLAYER").transform;
                }

                return _playerTrasnform;
            }
        }

        // �÷��̾� ��ġ����
        // �÷��̾� ���̿� ���߰�
        // ������ ���� �̵��ϰ�
        // �޼� �����տ� ���� �̵��Ѵ�
        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward 
                                      + PlayerBaseTransform.right;
        private bool isCanFire
        {
            get
            {
                // ��� �ӽ÷� ������ ���� �ٲ� �ڵ��̺��
                return FindObjectOfType<WeaponManagement>().transform == transform.parent;
            }
        }
        #endregion

        private InercioStatus _currentStatus = InercioStatus.Idle;

        private Transform _sticklyObjectTransform;
        private Vector3 _fireDir;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;
        }

        public override void FireWeapon()
        {
            if (isCanFire && _currentStatus != InercioStatus.Fire)
            {
                _fireDir = MainCameraTransform.forward;

                _currentStatus = InercioStatus.Fire;
            }
        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {
            _currentStatus = InercioStatus.Reset;
        }

        #region CollisionEvents
        public void PlayerCollisionEvent(GameObject obj)
        {
            ResetWeapon();
        }
        public void ATypeObjectCollisionEvent(Collision obj)
        {
            _currentStatus = InercioStatus.Stickly;

            _sticklyObjectTransform = obj.transform;
        }
        public void BTypeObjectCollisionEvent(Collision obj)
        {

        }
        #endregion

        private void Update()
        {
            if(_currentStatus == InercioStatus.Idle)
            {
                transform.position = HandPosition;
            }
            else if(_currentStatus == InercioStatus.Fire)
            {
                transform.position += _fireDir * Time.deltaTime * FireSpeed;
            }
            else if(_currentStatus == InercioStatus.Stickly)
            {

            }
            else if(_currentStatus == InercioStatus.Reset)
            {
                _currentStatus = InercioStatus.Idle;
            }
        }
    }
}