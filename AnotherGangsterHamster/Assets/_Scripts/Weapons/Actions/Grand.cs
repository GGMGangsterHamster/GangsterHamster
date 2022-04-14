using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Grand : WeaponAction
    {
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float resizeSpeed; // ũ�� ��ȯ�� �� ��� �ð�
        public float reboundPower;

        public Transform chargeBar;

        // �׷����� ũ�� ��ȯ �ܰ�
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
                        // ��¡ �Ǵ� UI �����ֱ�

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
                        // Ű�� ������ ���� Resize�� �̵�
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
            // ���⼭ ������ ���ǿ� �������� ���ϴ� ��� ���� �ڵ带 �������� ����
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

            // �̷� ���� ���ǿ� ������ �÷��̾�� �ݵ��� �ְ� �������� ��
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

        /// ���Ǽ���
        //          2�� ���� ũ�� ��ȯ �� ��
        //          �� 6�������� Ray�� After Size��ŭ�� ���̷� �߻���.
        //          �׸��� x, y, z������ ���� (x, -x), (y, -y), (z, -z) �� �� 3���� �������� �����ٵ�
        //          ���� �Ѱ��� �����̶� �Ѵ� BTYPEOBJECT�� �߰� "�ǰ�"
        //          �� �߰ߵ� obj �ΰ��� ���̸� ���ؼ�
        //          �� ���� ���� After Size���� �۴ٸ�
        //          �� ��� ũ�� ��ȯ�� ���߰� LosePower���·� ��ȯ�Ѵ�.
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