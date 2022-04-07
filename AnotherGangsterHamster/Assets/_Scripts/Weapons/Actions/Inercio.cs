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
        public float ReboundPower;

        // ���Ⱑ ���� �� �ִ� ��Ȳ�� ��Ƴ���
        private enum InercioStatus
        {
            Idle,
            Fire,
            Use,
            Stickly,
            LosePower,
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

        private GameObject _sticklyObject = null;
        private Rigidbody _sticklyObjectRigid = null;
        private Transform _sticklyObjBeforeParent;
        
        private Collider _myCollider;
        private Rigidbody _myRigid;

        private float _weaponUsedTime = 0f;
        private Vector3 _fireDir;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;

            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();
        }

        public override void FireWeapon()
        {
            if (isCanFire && _currentStatus != InercioStatus.Fire && _currentStatus != InercioStatus.Use)
            {
                _fireDir = MainCameraTransform.forward;

                _currentStatus = InercioStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;

            }
        }

        public override void UseWeapon()
        {
            if (_myRigid.constraints == RigidbodyConstraints.None)
                _myRigid.constraints = RigidbodyConstraints.FreezeAll;

            _currentStatus = InercioStatus.Use;
            _weaponUsedTime = 0f;
        }

        public override void ResetWeapon()
        {
            _currentStatus = InercioStatus.Idle;

            // 이너시오에 ATypeObject가 붙어있다면
            if(_sticklyObject != null)
            {
                _sticklyObject.transform.parent = _sticklyObjBeforeParent;
                _sticklyObjectRigid.constraints = RigidbodyConstraints.None;

                
                PlayerBaseTransform.GetComponent<Rigidbody>().velocity = 
                    (MainCameraTransform.position - transform.position).normalized 
                    * ReboundPower 
                    * _weaponUsedTime
                ;
            }

            _sticklyObject = null;
            _sticklyObjectRigid = null;
            _sticklyObjBeforeParent = null;
        }

        #region CollisionEvents
        public void PlayerCollisionEvent(GameObject col)
        {
            ResetWeapon();
        }
        public void ATypeObjectCollisionEnterEvent(GameObject col)
        {
            if (_sticklyObject != null)
            {
                return;
            }
            
            _currentStatus = InercioStatus.Stickly;
            _sticklyObject = col;
            _sticklyObjectRigid = _sticklyObject.GetComponent<Rigidbody>();
            _sticklyObjectRigid.constraints = RigidbodyConstraints.FreezeAll;
            _sticklyObjBeforeParent = col.transform.parent;
            _sticklyObject.transform.parent = transform;
        }
        public void ATypeObjectCollisionExitEvent(GameObject col)
        {
            // 일단 만들긴 만들었는데 쓸건지는 모르겟슴
        }
        public void BTypeObjectCollisionEnterEvent(GameObject col)
        {
            _myRigid.constraints = RigidbodyConstraints.None;
            _currentStatus = InercioStatus.LosePower;
        }
        #endregion

        private void Update()
        {
            switch(_currentStatus)
            {
                case InercioStatus.Idle:
                    if (!_myCollider.isTrigger)
                        _myCollider.isTrigger = true;
                    if (_myRigid.useGravity)
                        _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None)
                        _myRigid.constraints = RigidbodyConstraints.FreezeAll;

                    transform.position = HandPosition;
                    break;
                case InercioStatus.Fire:
                    transform.position += _fireDir * Time.deltaTime * FireSpeed;
                    break;
                case InercioStatus.Use:
                    if (_myRigid.useGravity)
                        _myRigid.useGravity = false;

                    transform.position += (MainCameraTransform.position - transform.position).normalized * Time.deltaTime * FireSpeed;
                    _weaponUsedTime += Time.deltaTime;
                    break;
                case InercioStatus.Stickly:

                    break;
                case InercioStatus.LosePower:
                    if (!_myRigid.useGravity)
                        _myRigid.useGravity = true;
                    if (_myRigid.constraints == RigidbodyConstraints.FreezeAll)
                        _myRigid.constraints = RigidbodyConstraints.None;
                    break;
            }
        }
    }
}