using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Grand : WeaponAction
    {
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float resizeSpeed; // 크기 변환할 때 드는 시간

        // 그랜드의 크기 변환 단계
        private enum GrandSizeLevel
        {
            OneGrade,
            TwoGrade,
            FourGrade,
        }

        private float fullChangeTime
        {
            get
            {
                if (_currentSizeLevel == GrandSizeLevel.OneGrade)
                    return 1f;
                else if (_currentSizeLevel == GrandSizeLevel.TwoGrade)
                    return 0.5f;
                else
                    return 0f;
            }
        }
        private bool isCanFire
        {
            get
            {
                return currentGrandStatus != GrandStatus.Use 
                    && currentGrandStatus != GrandStatus.LosePower 
                    && currentGrandStatus != GrandStatus.Fire 
                    && currentGrandStatus != GrandStatus.Resize;
            }
        }

        private Dictionary<GrandSizeLevel, float> _sizeLevelValue = new Dictionary<GrandSizeLevel, float>();

        private GrandSizeLevel _currentSizeLevel = GrandSizeLevel.OneGrade;
        [HideInInspector] public GrandStatus currentGrandStatus = GrandStatus.Idle;

        private KeyCode _useKeycode;

        private float _beforeWeaponSize = 0f;
        private float _currentLerpTime = 0f;
        private float _weaponUsedTime = 0f;

        private void Awake()
        {
            _sizeLevelValue.Add(GrandSizeLevel.OneGrade, 1f);
            _sizeLevelValue.Add(GrandSizeLevel.TwoGrade, 2f);
            _sizeLevelValue.Add(GrandSizeLevel.FourGrade, 4f);

            _weaponEnum = WeaponEnum.Grand;

            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;
        }

        public override void FireWeapon()
        {
            if (isCanFire)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _fireDir = MainCameraTransform.forward;

                transform.position = FirePosition;
                currentGrandStatus = GrandStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }
        public override void UseWeapon()
        {
            if (currentGrandStatus == GrandStatus.Idle || currentGrandStatus == GrandStatus.Resize) return;

            _myRigid.velocity = Vector3.zero;
            currentGrandStatus = GrandStatus.Use;
        }
        public override void ResetWeapon()
        {
            if(_currentSizeLevel == GrandSizeLevel.OneGrade && currentGrandStatus != GrandStatus.Resize)
            {
                transform.position = HandPosition;
                currentGrandStatus = GrandStatus.Idle;
            }
        }

        public override bool IsHandleWeapon()
        {
            return currentGrandStatus == GrandStatus.Idle;
        }

        private void Update()
        {
            switch(currentGrandStatus)
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
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case GrandStatus.Use:
                    _myRigid.velocity = Vector3.zero;
                    if (Input.GetKey(_useKeycode))
                    {
                        _weaponUsedTime += Time.deltaTime;

                        // 차징 되는 UI 보여주기
                        
                        if(_weaponUsedTime >= fullChangeTime)
                        {
                            MaxSizeLevel();
                            ResizeStart();
                            // 키를 누른지 1초가 지나면 Resize로 이동
                        }
                    }
                    else
                    {
                        NextSizeLevel();
                        ResizeStart();
                        // 키를 누르고 떼면 Resize로 이동
                    }
                    break;
                case GrandStatus.Resize:
                    if(_currentLerpTime >= resizeSpeed)
                    {
                        transform.localScale = Vector3.one * _sizeLevelValue[_currentSizeLevel];
                        transform.rotation = Quaternion.identity;

                        currentGrandStatus = GrandStatus.LosePower;
                    }
                    else
                    {
                        _currentLerpTime += Time.deltaTime;
                        transform.localScale = Vector3.one * Mathf.Lerp(_beforeWeaponSize, _sizeLevelValue[_currentSizeLevel], _currentLerpTime / resizeSpeed);
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, _currentLerpTime / resizeSpeed);
                    }
                    break;
                case GrandStatus.LosePower:
                    if (!_myRigid.useGravity)
                        _myRigid.useGravity = true;
                    if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                        _myRigid.constraints = RigidbodyConstraints.None;

                    break;
            }
        }

        private void NextSizeLevel()
        {
            int jumpLevel = 0;

            if (_weaponUsedTime >= 1f)
                jumpLevel = 2;
            else
                jumpLevel = 1;

            if (_currentSizeLevel + jumpLevel > GrandSizeLevel.FourGrade)
                _currentSizeLevel = GrandSizeLevel.OneGrade;
            else
                _currentSizeLevel += jumpLevel;
        }

        private void MaxSizeLevel()
        {
            Debug.Log("before : " + _currentSizeLevel.ToString());

            if (_currentSizeLevel == GrandSizeLevel.FourGrade)
                _currentSizeLevel = GrandSizeLevel.OneGrade;
            else
                _currentSizeLevel = GrandSizeLevel.FourGrade;

            Debug.Log("after : " + _currentSizeLevel.ToString());

        }

        private void ResizeStart()
        {
            currentGrandStatus = GrandStatus.Resize;
            _weaponUsedTime = 0f;
            _currentLerpTime = 0f;
            _beforeWeaponSize = transform.localScale.x;

            _myRigid.angularVelocity = Vector3.zero;
        }
    }
}