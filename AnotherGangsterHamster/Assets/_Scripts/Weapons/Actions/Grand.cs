using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �ڵ�� 100�� ����
// ���� �ڵ�� 400�� �̻�

namespace Weapons.Actions
{
    [RequireComponent(typeof(GravityAffectedObject))]
    public class Grand : WeaponAction
    {
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float resizeSpeed; // ũ�� ��ȯ�� �� ��� �ð�
        public float reboundPower;
        public float alphaSensorValue; // ������Ʈ�� ���������� �Ÿ�

        private Transform chargeBar;
        private Transform _dropPoint;

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

        private AlphaSensor _sensor;

        private Dictionary<GrandSizeLevel, float> _sizeLevelValue = new Dictionary<GrandSizeLevel, float>();

        private GrandSizeLevel _currentSizeLevel = GrandSizeLevel.OneGrade;
        private GrandSizeLevel _beforeSizeLevel = GrandSizeLevel.OneGrade;
        [HideInInspector] public GrandStatus _currentGrandStatus = GrandStatus.Idle;

        private Quaternion lerpQuaternion;
        private KeyCode _useKeycode;

        private float _beforeWeaponSize = 0f;
        private float _currentLerpTime = 0f;
        private float _weaponUsedTime = 0f;

        private Vector3 beforePos;
        private Vector3 afterPos;

        private float _fireCoolTime;

        private new void Awake()
        {
            base.Awake();
            _weaponEnum = WeaponEnum.Grand;

            _sizeLevelValue.Add(GrandSizeLevel.OneGrade, 1f);
            _sizeLevelValue.Add(GrandSizeLevel.TwoGrade, 2f);
            _sizeLevelValue.Add(GrandSizeLevel.FourGrade, 4f);

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;

            chargeBar = GameObject.Find("ChargeBar").transform;

            _sensor = GetComponent<AlphaSensor>();
            _dropPoint = transform.GetChild(0);

            Debug.Log(_dropPoint.name);
        }

        private void Start()
        {
            // ���� �÷��̾���� �Ÿ��� alphaSensorValue���� �����ٸ� ������ �ø���.
            _sensor.requirement += () =>
            {
                return alphaSensorValue > Vector3.Distance(PlayerBaseTransform.position, transform.position) - _sizeLevelValue[_currentSizeLevel];
            };
        }

        public override void FireWeapon()
        {
            if(Time.time - _fireCoolTime > 0.3f) _fireCoolTime = Time.time;
            else return; 

            if (_currentGrandStatus == GrandStatus.Idle)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _dropPoint.gameObject.SetActive(true);

                _fireDir = MainCameraTransform.forward;

                if (Vector3.Angle(_fireDir, -PlayerBaseTransform.up) < 37.5f)
                {
                    bool b = Physics.Raycast(PlayerTrasnform.position - (PlayerBaseTransform.up * 0.88f), _fireDir, out RaycastHit hit);
                    float dist = Vector3.Distance(PlayerBaseTransform.position, hit.point);


                    if (b && dist < 0.2f
                    && hit.transform.CompareTag("BTYPEOBJECT"))
                    {
                        transform.position = FirePosition;
                        PlayerBaseTransform.position += PlayerBaseTransform.up;
                    }
                    else
                    {
                        if(dist >= 0.9f)
                        {
                            transform.position = FirePosition - (PlayerBaseTransform.up * 0.9f);
                        }
                        else
                        {
                            transform.position = FirePosition - (PlayerBaseTransform.up * dist);
                            PlayerBaseTransform.position += PlayerBaseTransform.up * (1.2f - dist);
                            PlayerBaseTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        }
                    }
                }
                else if (Physics.Raycast(MainCameraTransform.position, _fireDir, out RaycastHit hit2, 1.5f) && hit2.transform.CompareTag("BTYPEOBJECT"))
                {
                    float dist = (1.5f - Vector3.Distance(MainCameraTransform.position, hit2.point));

                    transform.position = FirePosition;
                    transform.position += new Vector3(PlayerBaseTransform.localScale.x * hit2.normal.x * (dist / 1.4f),
                                                      PlayerBaseTransform.localScale.y * hit2.normal.y * (dist / 1.4f),
                                                      PlayerBaseTransform.localScale.z * hit2.normal.z * (dist / 1.4f));

                    PlayerBaseTransform.position += hit2.normal * (dist + 0.5f);
                }
                else
                {
                    transform.position = FirePosition;
                }

                _currentGrandStatus = GrandStatus.Fire;

                _myCollider.isTrigger = false;
                (_myCollider as BoxCollider).center = Vector3.zero;
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
            if(_currentGrandStatus != GrandStatus.Resize)
            {
                _currentSizeLevel = GrandSizeLevel.OneGrade;
                transform.localScale = Vector3.one * _sizeLevelValue[_currentSizeLevel];

                chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ?
                                                            0 :
                                                            _sizeLevelValue[_currentSizeLevel] * 0.25f, 1, 1);

                transform.position = HandPosition;
                transform.rotation = Quaternion.identity;
                _myRigid.angularVelocity = Vector3.zero;
                _currentGrandStatus = GrandStatus.Idle;
                _weaponUsedTime = 0f;
                _myRigid.constraints = RigidbodyConstraints.None;
            }
        }

        public override bool IsHandleWeapon()
        {
            return _currentGrandStatus == GrandStatus.Idle;
        }

        #region CollisionEvents
        public void BTypeObjCollisionEnterEvent(GameObject obj)
        {
            if(_currentGrandStatus != GrandStatus.Resize &&
               _currentGrandStatus != GrandStatus.Use)
            {
                _currentGrandStatus = GrandStatus.LosePower;
            }
        }

        public void BTypeObjCollisionStayEvent(GameObject obj)
        {
            if (_currentGrandStatus != GrandStatus.Resize &&
                _currentGrandStatus != GrandStatus.Use &&
                _currentGrandStatus != GrandStatus.LosePower)
            {
                _currentGrandStatus = GrandStatus.LosePower;
            }
        }
        #endregion

        private void Update()
        {
            switch(_currentGrandStatus)
            {
                case GrandStatus.Idle:
                    (_myCollider as BoxCollider).center = Vector3.one * short.MaxValue;
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
                case GrandStatus.Resize: // ũ�� ��ȯ ����
                    if(_currentLerpTime >= resizeSpeed)
                    {
                        transform.localScale = Vector3.one * _sizeLevelValue[_currentSizeLevel];
                        transform.rotation = Quaternion.identity;
                        _currentGrandStatus = GrandStatus.LosePower;
                        _myRigid.constraints = RigidbodyConstraints.None;

                        _myRigid.velocity = Vector3.zero;
                        _myRigid.angularVelocity = Vector3.zero;
                    }
                    else
                    {
                        _currentLerpTime += Time.deltaTime;
                        transform.localScale = Vector3.one * Mathf.Lerp(_beforeWeaponSize, _sizeLevelValue[_currentSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                        transform.rotation = Quaternion.Lerp(transform.rotation, lerpQuaternion, Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                        transform.position = Vector3.Lerp(beforePos, afterPos, Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                    }
                    break;
                case GrandStatus.LosePower:
                    if (_myRigid.constraints == RigidbodyConstraints.FreezePosition) _myRigid.constraints = RigidbodyConstraints.None;
                    
                    break;
            }

            ShowDropPoint();
        }

        private void NextSizeLevel()
        {
            int jumpLevel = 0;

            if (_weaponUsedTime >= 0.65f)
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
            if (!CanResize(Vector3.up) ||
               !CanResize(Vector3.right) ||
               !CanResize(Vector3.forward))
            {
                return;
            }

            // ũ�� ��ȯ �� �ʱ� �۾�
            float x, y, z;

            _currentGrandStatus = GrandStatus.Resize;
            _weaponUsedTime = 0f;
            _currentLerpTime = 0f;
            _beforeWeaponSize = transform.localScale.x;
            chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ? 0 : _sizeLevelValue[_currentSizeLevel] * 0.25f
                                                , 1, 1);

            // �̷� ���� ���ǿ� ������ �÷��̾�� �ݵ��� �ְ� �������� ��
            // 1. �۾����� ����� ���� ����
            // 2. �÷��̾ ������ ������ ���;� ������
            if (_sizeLevelValue[_currentSizeLevel] - _beforeWeaponSize > 0)
            {
                if ((_sizeLevelValue[_currentSizeLevel] / 2) > Vector3.Distance(transform.position, PlayerBaseTransform.position) - (PlayerBaseTransform.localScale.x + PlayerBaseTransform.localScale.y) / 3)
                {
                    Vector3 reboundDir = (PlayerBaseTransform.position - transform.position).normalized;
                    float rebound = (_sizeLevelValue[_currentSizeLevel] - _beforeWeaponSize) * reboundPower;

                    float maxValue = Mathf.Max(Mathf.Abs(reboundDir.x),
                                     Mathf.Max(Mathf.Abs(reboundDir.y),
                                               Mathf.Abs(reboundDir.z)));

                    x = maxValue == Mathf.Abs(reboundDir.x) ? rebound * Mathf.Sign(reboundDir.x) : 0;
                    y = maxValue == Mathf.Abs(reboundDir.y) ? rebound * Mathf.Sign(reboundDir.y) : 0;
                    z = maxValue == Mathf.Abs(reboundDir.z) ? rebound * Mathf.Sign(reboundDir.z) : 0;

                    //PlayerBaseTransform.GetComponent<Rigidbody>().velocity = (transform.right * x) + (transform.up * y) + (transform.forward * z); // ������ ������ ���� �ݵ� �ִ� ��
                    PlayerBaseTransform.GetComponent<Rigidbody>().velocity = new Vector3(x, y, z); // ������ ������ �����ϰ� World ��ǥ�� �ݵ� �ִ°�
                    
                    Player.Damage(weaponDamage);
                }
            }

            x = Mathf.Floor(transform.rotation.eulerAngles.x % 90);
            y = Mathf.Floor(transform.rotation.eulerAngles.y % 90);
            z = Mathf.Floor(transform.rotation.eulerAngles.z % 90);

            lerpQuaternion = Quaternion.Euler(x < 45 ? -x : 90 - x,
                                              y < 45 ? -y : 90 - y,
                                              z < 45 ? -z : 90 - z);

            lerpQuaternion = Quaternion.Euler(transform.rotation.eulerAngles.x + lerpQuaternion.eulerAngles.x,
                                                transform.rotation.eulerAngles.y + lerpQuaternion.eulerAngles.y,
                                                transform.rotation.eulerAngles.z + lerpQuaternion.eulerAngles.z);
            
            beforePos = transform.position;
            afterPos = transform.position;
            ReadjustmentPos(Vector3.right);
            ReadjustmentPos(Vector3.up);
            ReadjustmentPos(Vector3.forward);

            _myRigid.angularVelocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.FreezeRotation;
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
            if (Physics.Raycast(transform.position, checkDir, out RaycastHit plusHit, _sizeLevelValue[_currentSizeLevel]) &&
                Physics.Raycast(transform.position, -checkDir, out RaycastHit minusHit, _sizeLevelValue[_currentSizeLevel]))
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
        private void ReadjustmentPos(Vector3 checkDir)
        {
            float curSize = _sizeLevelValue[_currentSizeLevel] / 2;
            float plusAxisDist = GetDistance(checkDir);
            float minusAxisDist = GetDistance(-checkDir);

            if (plusAxisDist < curSize)
            {
                float padding = curSize - plusAxisDist;

                if(minusAxisDist > curSize + padding)
                {
                    afterPos += -checkDir * padding;
                    // padding ��ŭ �̵�
                }
            }
            else if (minusAxisDist < curSize)
            {
                float padding = curSize - minusAxisDist;

                if (plusAxisDist > curSize + padding)
                {
                    afterPos += checkDir * padding;
                    // padding ��ŭ �̵�
                }
            }
            else
            {
                // ������ ���ٴ� ��!!
            }
        }
        private float GetDistance(Vector3 dir)
        {
            if (Physics.BoxCast(transform.position, Vector3.one * (_sizeLevelValue[_beforeSizeLevel] / 2 - _sizeLevelValue[_beforeSizeLevel] / 10), dir, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("BTYPEOBJECT") && !hit.collider.isTrigger)
                {
                    return Vector3.Distance(hit.point, transform.position);
                }
            }

            return float.MaxValue;
        }

        private void ShowDropPoint()
        {
            if(_currentGrandStatus == GrandStatus.Fire)
            {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down);
                float minDistance = float.MaxValue;
                int index = -1;

                if (hits != null)
                {

                    for(int i = 0; i < hits.Length; i++)
                    {
                        if(hits[i].transform.CompareTag("BTYPEOBJECT") && hits[i].distance < minDistance)
                        {
                            minDistance = hits[i].distance;
                            index = i;
                        }
                    }

                    if(index != -1)
                    {
                        RaycastHit hit = hits[index];

                        _dropPoint.position = hit.point + Vector3.up * 0.05f;
                    }
                }
            }
            else
            {
                _dropPoint.gameObject.SetActive(false);
            }
        }
    }
}