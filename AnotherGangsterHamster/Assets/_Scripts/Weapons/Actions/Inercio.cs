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
        }

        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTrasnform;

        #region Propertys
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
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);
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

        private Collision _sticklyObjectCollision;
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
            _currentStatus = InercioStatus.Use;
        }

        public override void ResetWeapon()
        {
            _currentStatus = InercioStatus.Idle;

            _sticklyObjectCollision = null;
        }

        #region CollisionEvents
        public void PlayerCollisionEvent(Collision col)
        {
            ResetWeapon();
        }
        public void ATypeObjectCollisionEnterEvent(Collision col)
        {
            if (_sticklyObjectCollision != null && _sticklyObjectCollision.gameObject == col.gameObject)
            {
                return;
            }
            
            _currentStatus = InercioStatus.Stickly;
            _sticklyObjectCollision = col;
        }
        public void ATypeObjectCollisionExitEvent(Collision col)
        {

        }
        public void BTypeObjectCollisionEnterEvent(Collision col)
        {
            
        }
        #endregion

        private void Update()
        {
            switch(_currentStatus)
            {
                case InercioStatus.Idle:
                    transform.position = HandPosition;
                    break;
                case InercioStatus.Fire:
                    transform.position += _fireDir * Time.deltaTime * FireSpeed;
                    break;
                case InercioStatus.Use:
                    transform.position += (MainCameraTransform.position - transform.position).normalized * Time.deltaTime * FireSpeed;


                    break;
                case InercioStatus.Stickly:
                    Debug.Log("Stickly ����");
                    break;
            }
        }
    }
}