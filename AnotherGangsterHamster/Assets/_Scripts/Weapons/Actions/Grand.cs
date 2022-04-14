using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Grand : WeaponAction
    {
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float resizeSpeed; // 크기 변환할 때 드는 시간
        public float reboundPower;

        public Transform chargeBar;

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

        private float ChargeBarValue
        {
            get
            {
                if (_currentSizeLevel == GrandSizeLevel.OneGrade)
                    return _weaponUsedTime;
                else if (_currentSizeLevel == GrandSizeLevel.TwoGrade)
                    return 0.5f + _weaponUsedTime;
                else
                    return 1f;
            }
        }

        private Dictionary<GrandSizeLevel, float> _sizeLevelValue = new Dictionary<GrandSizeLevel, float>();

        private GrandSizeLevel _currentSizeLevel = GrandSizeLevel.OneGrade;
        private GrandSizeLevel _beforeSizeLevel = GrandSizeLevel.OneGrade;
        [HideInInspector] public GrandStatus _currentGrandStatus = GrandStatus.Idle;

        private KeyCode _useKeycode;

        private float _beforeWeaponSize = 0f;
        private float _currentLerpTime = 0f;
        private float _weaponUsedTime = 0f;

        private new void Awake()
        {
            base.Awake();

            _sizeLevelValue.Add(GrandSizeLevel.OneGrade, 1f);
            _sizeLevelValue.Add(GrandSizeLevel.TwoGrade, 2f);
            _sizeLevelValue.Add(GrandSizeLevel.FourGrade, 4f);

            _weaponEnum = WeaponEnum.Grand;

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;
        }

        public override void FireWeapon()
        {
            if (_currentGrandStatus != GrandStatus.Use
                    && _currentGrandStatus != GrandStatus.LosePower
                    && _currentGrandStatus != GrandStatus.Fire
                    && _currentGrandStatus != GrandStatus.Resize)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _fireDir = MainCameraTransform.forward;

                transform.position = FirePosition;
                _currentGrandStatus = GrandStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }
        public override void UseWeapon()
        {
            if (_currentGrandStatus == GrandStatus.Idle || _currentGrandStatus == GrandStatus.Resize) return;

            _myRigid.velocity = Vector3.zero;
            _beforeSizeLevel = _currentSizeLevel;
            _currentGrandStatus = GrandStatus.Use;
        }
        public override void ResetWeapon()
        {
            if(_currentSizeLevel == GrandSizeLevel.OneGrade && _currentGrandStatus != GrandStatus.Resize)
            {
                transform.position = HandPosition;
                _currentGrandStatus = GrandStatus.Idle;
            }
        }

        public override bool IsHandleWeapon()
        {
            return _currentGrandStatus == GrandStatus.Idle;
        }

        #region CollisionEvents
        public void PlayerCollisionEnterEvent(GameObject obj)
        {

        }
        #endregion

        private void Update()
        {
            switch(_currentGrandStatus)
            {
                case GrandStatus.Idle:
                    if (!_myCollider.isTrigger) _myCollider.isTrigger = true;
                    if (_myRigid.useGravity) _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;

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

                        chargeBar.localScale = new Vector3(ChargeBarValue, 1, 1);
                        // 차징 되는 UI 보여주기

                        if (_weaponUsedTime >= fullChangeTime)
                        {
                            MaxSizeLevel();
                            ResizeStart();
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

                        _currentGrandStatus = GrandStatus.LosePower;
                    }
                    else
                    {
                        _currentLerpTime += Time.deltaTime;
                        transform.localScale = Vector3.one * Mathf.Lerp(_beforeWeaponSize, _sizeLevelValue[_currentSizeLevel], _currentLerpTime / resizeSpeed);
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, _currentLerpTime / resizeSpeed);
                    }
                    break;
                case GrandStatus.LosePower:
                    if (!_myRigid.useGravity) _myRigid.useGravity = true;
                    if (_myRigid.constraints == RigidbodyConstraints.FreezePosition) _myRigid.constraints = RigidbodyConstraints.None;

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
            if (_currentSizeLevel == GrandSizeLevel.FourGrade)
                _currentSizeLevel = GrandSizeLevel.OneGrade;
            else
                _currentSizeLevel = GrandSizeLevel.FourGrade;
        }

        private void ResizeStart()
        {
            // 여기서 정해진 조건에 충족하지 못하는 경우 밑의 코드를 실행하지 못함
            if(!CanResize(transform.up) || 
               !CanResize(transform.right) || 
               !CanResize(transform.forward))
            {
                return;
            }


            _currentGrandStatus = GrandStatus.Resize;
            _weaponUsedTime = 0f;
            _currentLerpTime = 0f;
            _beforeWeaponSize = transform.localScale.x;
            chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ? 0 : _sizeLevelValue[_currentSizeLevel] * 0.25f
                                                , 1, 1);

            // 이런 저런 조건에 맞으면 플레이어에게 반동을 주고 데미지도 줌
            if (_sizeLevelValue[_currentSizeLevel] - _beforeWeaponSize > 0)
            {
                if((_sizeLevelValue[_currentSizeLevel] / 2) > Vector3.Distance(transform.position, PlayerBaseTransform.position))
                {
                    Vector3 reboundDir = (PlayerBaseTransform.position - transform.position).normalized;
                    float rebound = (_sizeLevelValue[_currentSizeLevel] - _beforeWeaponSize) * reboundPower;

                    Debug.Log(rebound);
                    Player.Damage(weaponDamage);

                    PlayerBaseTransform.GetComponent<Rigidbody>().velocity = reboundDir * rebound;
                }
            }

            _myRigid.angularVelocity = Vector3.zero;
        }

        /// 조건설명
        //          2번 무기 크기 변환 할 때
        //          총 6방향으로 Ray를 After Size만큼의 길이로 발사함.
        //          그리고 x, y, z축으로 각각 (x, -x), (y, -y), (z, -z) 로 총 3개의 묶음으로 나뉠텐데
        //          만약 한개의 묶음이라도 둘다 BTYPEOBJECT가 발견 "되고"
        //          그 발견된 obj 두개의 길이를 더해서
        //          그 더한 값이 After Size보다 작다면
        //          그 즉시 크기 변환을 멈추고 LosePower상태로 변환한다.
        ///
        private bool CanResize(Vector3 checkDir)
        {
            if (UnityEngine.Physics.Raycast(transform.position, checkDir, out RaycastHit plusHit, _sizeLevelValue[_currentSizeLevel]) &&
                UnityEngine.Physics.Raycast(transform.position, -checkDir, out RaycastHit minusHit, _sizeLevelValue[_currentSizeLevel]))
            {

                if (plusHit.transform.CompareTag("BTYPEOBJECT") && minusHit.transform.CompareTag("BTYPEOBJECT"))
                {
                    if (Vector3.Distance(transform.position, plusHit.point) + 
                        Vector3.Distance(transform.position, minusHit.point) < _sizeLevelValue[_currentSizeLevel])
                    {
                        _currentSizeLevel = _beforeSizeLevel;
                        _currentGrandStatus = GrandStatus.LosePower;
                        _weaponUsedTime = 0f;
                        _currentLerpTime = 0f;

                        chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ? 
                                                            0 : 
                                                            _sizeLevelValue[_currentSizeLevel] * 0.25f, 1, 1);

                        return false;
                    }
                }
            }

            return true;
        }
    }
}