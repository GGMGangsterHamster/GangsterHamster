using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class Gravito : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public float fireSpeed;
        public float gravityChangeSpeed;
        private enum GravitoStatus
        {
            Idle,
            Fire,
            Use,
            Stickly,
            ChangeGravity,
        }

        #region Propertys

        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTrasnform;

        private Transform MainCameraTransform
        {
            get
            {
                if (_mainCameraTransform == null)
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
                if (_playerBaseTransform == null)
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

        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);


        private Vector3 FirePosition => MainCameraTransform.position + MainCameraTransform.forward;
        #endregion

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle;
        
        private Collider _myCollider;
        private Rigidbody _myRigid;
        
        private Vector3 _fireDir;

        private void Awake()
        {
            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            _weaponEnum = WeaponEnum.Gravito;
        }

        public override void FireWeapon()
        {
            if(_currentGravitoStatus != GravitoStatus.Fire && _currentGravitoStatus != GravitoStatus.Use)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;
                
                _fireDir = MainCameraTransform.forward;
                transform.position = FirePosition;
                _currentGravitoStatus = GravitoStatus.Fire;
            }
        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {
            transform.position = HandPosition;
            _currentGravitoStatus = GravitoStatus.Idle;

            // �߷� �ʱ�ȭ
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (!obj.TryGetComponent(out BoxCollider boxCol))
            {
                // ���⼭ �̰����� ������ �ȵȴٴ� ���� �������� ��
                return;
            }

            if(_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (!obj.TryGetComponent(out BoxCollider boxCol))
            {
                // ���⼭ �̰����� ������ �ȵȴٴ� ���� �������� ��
                return;
            }

            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        private void Update()
        {
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    if (!_myCollider.isTrigger)
                        _myCollider.isTrigger = true;
                    if (_myRigid.useGravity)
                        _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None)
                        _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire:
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case GravitoStatus.Stickly:
                    // �� ���¿� �ɷ� ���� ���� ���⿡ ���� �߷��� ��ȯ�Ǿ�� ��.
                    // ���� ���� ����.. ����� �κ�
                    break;
            }
        }

        private void Stop()
        {
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;


        }
    }
}