using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Inercio : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public float DefaultFireSpeed;
        public float DefaultReboundPower;
        public float TimeReboundPower; // 무기로 인해 반동 받을 때의 힘

        // 이너시오가 가질 수 있는 상태들
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

        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward 
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);
        private bool isCanFire
        {
            get
            {
                return _currentInercioStatus != InercioStatus.Use;
            }
        }
        #endregion

        private WeaponManagement _weaponManagement;

        private InercioStatus _currentInercioStatus = InercioStatus.Idle;

        private GameObject _sticklyObject = null;
        private Rigidbody _sticklyObjectRigid = null;
        private Transform _sticklyObjBeforeParent;
        
        private Collider _myCollider;
        private Rigidbody _myRigid;

        private float _weaponUsedTime = 0f;
        private float _fireTime = 0f; // 발사하고 나서 초마다 FireAcceleration만큼 증가하는 변수
        private Vector3 _fireDir;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;

            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            _weaponManagement = transform.parent.GetComponent<WeaponManagement>();
        }

        public override void FireWeapon()
        {
            if (isCanFire && _currentInercioStatus != InercioStatus.Fire && 
                _currentInercioStatus != InercioStatus.Stickly &&
                _currentInercioStatus != InercioStatus.LosePower
                )
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezeAll)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _fireDir = MainCameraTransform.forward;
                _fireTime = DefaultFireSpeed;

                _currentInercioStatus = InercioStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }

        public override void UseWeapon()
        {
            if (_currentInercioStatus == InercioStatus.Idle) return;

            _myRigid.constraints = RigidbodyConstraints.None;
            _currentInercioStatus = InercioStatus.Use;
            _weaponUsedTime = DefaultReboundPower;
        }

        public override void ResetWeapon()
        {
            if (_weaponManagement.GetCurrentWeapon() != _weaponEnum) gameObject.SetActive(false);

            _currentInercioStatus = InercioStatus.Idle;

            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
                        _myCollider.isTrigger = true;

            // 이너시오에 ATypeObject가 붙어있다면
            if (_sticklyObject != null)
            {
                _sticklyObject.transform.parent = _sticklyObjBeforeParent;
                _sticklyObjectRigid.constraints = RigidbodyConstraints.None;

                if (Vector3.Distance(transform.position, PlayerBaseTransform.position) <= 2.5f 
                    && _myRigid.constraints != RigidbodyConstraints.FreezeAll)
                {
                    PlayerBaseTransform.GetComponent<Rigidbody>().velocity =
                       (MainCameraTransform.position - transform.position).normalized
                       * TimeReboundPower
                       * _weaponUsedTime;
                }
            }

            _sticklyObject = null;
            _sticklyObjectRigid = null;
            _sticklyObjBeforeParent = null;
        }

        public override bool IsHandleWeapon()
        {
            return _currentInercioStatus == InercioStatus.Idle;
        }

        #region CollisionEvents
        public void PlayerCollisionEvent(GameObject obj)
        {
            _myRigid.velocity = Vector3.zero;
            ResetWeapon();
        }
        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (_sticklyObject != null)
            {
                return;
            }

            _currentInercioStatus = InercioStatus.Stickly;
            _sticklyObject = obj; 
            _sticklyObjBeforeParent = obj.transform.parent;
            _sticklyObject.transform.parent = transform;

            _sticklyObjectRigid = _sticklyObject.GetComponent<Rigidbody>();
            _sticklyObjectRigid.constraints = RigidbodyConstraints.FreezeAll;
            _sticklyObjectRigid.velocity = Vector3.zero;
            _sticklyObjectRigid.angularVelocity = Vector3.zero;
        }
        public void ATypeObjectCollisionExitEvent(GameObject obj)
        {
            // 일단 만들긴 만들었는데 쓸건지는 모르겟슴
        }
        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            _myRigid.constraints = RigidbodyConstraints.None;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _currentInercioStatus = InercioStatus.LosePower;

            if (_sticklyObject != null)
            {
                _sticklyObject.transform.parent = _sticklyObjBeforeParent;
                _sticklyObjectRigid.constraints = RigidbodyConstraints.None;
                _sticklyObject = null;
                _sticklyObjectRigid = null;
                _sticklyObjBeforeParent = null;

            }
        }
        #endregion

        private void Update()
        {
            switch(_currentInercioStatus)
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
                    _myRigid.velocity = _fireDir * DefaultFireSpeed;
                    break;
                case InercioStatus.Use:
                    {
                        if (_myRigid.useGravity)
                            _myRigid.useGravity = false;

                        _fireDir = (MainCameraTransform.position - transform.position).normalized;
                        _weaponUsedTime += Time.deltaTime;

                        _myRigid.velocity = _fireDir * DefaultFireSpeed;

                        if(_sticklyObject != null)
                        {
                            if (_sticklyObject.TryGetComponent(out Grand grand))
                            {
                                if (grand.currentGrandStatus == Grand.GrandStatus.Resize)
                                {
                                    ResetWeapon();
                                }
                            }
                        }

                        break;
                    }
                case InercioStatus.Stickly:
                    {
                        if(_myRigid.constraints == RigidbodyConstraints.None)
                        {
                            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
                            _myRigid.velocity = Vector3.zero;
                            _myRigid.angularVelocity = Vector3.zero;
                        }
                        if (_sticklyObject.TryGetComponent(out Grand grand))
                        {
                            if (grand.currentGrandStatus == Grand.GrandStatus.Resize)
                            {
                                ResetWeapon();
                            }
                        }
                        break;
                    }
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