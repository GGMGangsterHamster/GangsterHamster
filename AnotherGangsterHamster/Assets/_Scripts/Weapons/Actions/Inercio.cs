using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Inercio : WeaponAction
    {
        public float DefaultReboundPower;
        public float TimeReboundPower; // 무기로 인해 반동 받을 때의 힘

        private WeaponManagement _weaponManagement;

        private InercioStatus _currentInercioStatus = InercioStatus.Idle;

        private GameObject _sticklyObject = null;
        private Rigidbody _sticklyObjectRigid = null;
        private Transform _sticklyObjBeforeParent;

        private float _fireDelay = 0f; // 무기를 발사하고 나서 바로 회수되는 일 없도록 만든 변수
        private float _weaponUsedTime = 0f;
        private bool isCanFire = true;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;

            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            _weaponManagement = transform.parent.GetComponent<WeaponManagement>();
        }

        #region Actions
        public override void FireWeapon()
        {
            if (isCanFire && _currentInercioStatus != InercioStatus.Use && 
                _currentInercioStatus != InercioStatus.Fire &&
                _currentInercioStatus != InercioStatus.Stickly &&
                _currentInercioStatus != InercioStatus.LosePower
                )
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezeAll)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _fireDir = MainCameraTransform.forward;

                transform.position = FirePosition;
                _currentInercioStatus = InercioStatus.Fire;
                _fireDelay = 0f;

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

            Invoke("SetTrigger", 0.1f);
        }

        /// <summary>
        /// Invoke용 함수
        /// </summary>
        private void SetTrigger()
        {
            _myCollider.isTrigger = true;
        }

        public override void ResetWeapon()
        {
            if (_weaponManagement.GetCurrentWeapon() != _weaponEnum) gameObject.SetActive(false);

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

                    _sticklyObject.transform.position = transform.position;
                }
            }

            _currentInercioStatus = InercioStatus.Idle;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _myCollider.isTrigger = true;
            
            _sticklyObject = null;
            _sticklyObjectRigid = null;
            _sticklyObjBeforeParent = null;
        }

        public override bool IsHandleWeapon()
        {
            return _currentInercioStatus == InercioStatus.Idle;
        }
        #endregion

        #region CollisionEvents
        public void PlayerCollisionEvent(GameObject obj)
        {
            if (_fireDelay < 0.4f && _currentInercioStatus == InercioStatus.Fire) return;

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

        #region TriggerEvents
        public void PlayerTriggerEvent(GameObject obj)
        {
            _myRigid.velocity = Vector3.zero;
            ResetWeapon();
        }

        public void ObjectATriggerEvent(GameObject obj)
        {
            if (_sticklyObject != null || _currentInercioStatus == InercioStatus.Idle)
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

        public void ObjectBTriggerEnterEvent(GameObject obj)
        {
            if (_currentInercioStatus == InercioStatus.Idle)
            {
                isCanFire = false;
                return;
            }

            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            _currentInercioStatus = InercioStatus.LosePower;

            _myCollider.isTrigger = false;

            if (_sticklyObject != null)
            {
                _sticklyObject.transform.parent = _sticklyObjBeforeParent;
                _sticklyObjectRigid.constraints = RigidbodyConstraints.None;
                _sticklyObject = null;
                _sticklyObjectRigid = null;
                _sticklyObjBeforeParent = null;
            }
        }

        public void ObjectBTriggetExitEvent(GameObject obj)
        {
            if (_currentInercioStatus == InercioStatus.Idle)
            {
                isCanFire = true;
                return;
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
                    _fireDelay += Time.deltaTime;
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case InercioStatus.Use:
                    {
                        if (_myRigid.useGravity)
                            _myRigid.useGravity = false;

                        _fireDir = (MainCameraTransform.position - transform.position).normalized;
                        _weaponUsedTime += Time.deltaTime;

                        _myRigid.velocity = _fireDir * fireSpeed;

                        if(_sticklyObject != null)
                        {
                            if (_sticklyObject.TryGetComponent(out Grand grand))
                            {
                                if (grand.currentGrandStatus == GrandStatus.Resize)
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
                            if (grand.currentGrandStatus == GrandStatus.Resize)
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