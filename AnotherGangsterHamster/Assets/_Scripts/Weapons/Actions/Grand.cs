using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Grand : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float FireSpeed;
        public float ResizeSpeed; // 크기 변환할 때 드는 시간

        // 그랜드의 크기 변환 단계
        private enum GrandSizeLevel
        {
            OneGrade,
            TwoGrade,
            FourGrade,
        }

        // 그랜드가 가질 수 있는 상태들
        private enum GrandStatus
        {
            Idle,
            Fire,
            Use,
            Resize,
            LosePower,
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
        private bool isCanFire
        {
            get
            {
                return FindObjectOfType<WeaponManagement>().transform == transform.parent && _currentGrandStatus != GrandStatus.Use;
            }
        }
        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);
        #endregion

        private GrandSizeLevel _currentSizeLevel = GrandSizeLevel.OneGrade;
        private GrandStatus _currentGrandStatus = GrandStatus.Idle;

        private KeyCode _useKeycode;

        private Collider _myCollider;
        private Rigidbody _myRigid;

        private float _weaponUsedTime = 0f;
        private Vector3 _fireDir;


        private void Awake()
        {
            _weaponEnum = WeaponEnum.Grand;

            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;
        }

        public override void FireWeapon()
        {
            if (isCanFire && _currentGrandStatus != GrandStatus.Fire)
            {
                _fireDir = MainCameraTransform.forward;

                _currentGrandStatus = GrandStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }
        public override void UseWeapon()
        {
            if (isCanFire) return;

            _currentGrandStatus = GrandStatus.Use;
        }
        public override void ResetWeapon()
        {
            _currentGrandStatus = GrandStatus.Idle;
        }

        private void Update()
        {
            switch(_currentGrandStatus)
            {
                case GrandStatus.Idle:
                    if (!_myCollider.isTrigger)
                        _myCollider.isTrigger = true;
                    if (_myRigid.useGravity)
                        _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None)
                        _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GrandStatus.Fire:
                    transform.position += _fireDir * Time.deltaTime * FireSpeed;
                    break;
                case GrandStatus.Use:
                    if(Input.GetKey(_useKeycode))
                    {
                        _weaponUsedTime += Time.deltaTime;

                        // 차징 되는 UI 보여주기
                        
                        if(_weaponUsedTime >= 1f)
                        {
                            _currentSizeLevel = GrandSizeLevel.FourGrade;
                            _currentGrandStatus = GrandStatus.Resize;
                            _weaponUsedTime = 0f;
                            // 키를 누른지 1초가 지나면 Resize로 이동
                        }
                    }
                    else
                    {
                        _weaponUsedTime = 0f;
                        // 키를 누르고 떼면 Resize로 이동
                    }
                    break;
                case GrandStatus.Resize:
                    
                    break;
            }
        }
    }
}