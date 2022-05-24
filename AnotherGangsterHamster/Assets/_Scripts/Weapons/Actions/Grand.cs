using Characters.Player.Move;
using Matters.Gravity;
using Matters.Velocity;
using Objects;
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
        public float alphaToZeroSpeed;

        public bool IsCanChangeTwoStep
        {
            get => isCanChangeTwoStep;
            set => isCanChangeTwoStep = value;
        }

        private Transform chargeBar;
        private Transform _dropPoint;
        private LineRenderer _dropLineRenderer;

        private CollisionInteractableObject _enterCollision;
        private CollisionStayInteractableObject _stayCollision;
        
        // WeaponEvents Singleton ���� ���ϱ� ����
        private WeaponEvents _events;

        private FollowGroundPos _playerFollow;

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

        private GameObject grandLv1Model;
        private GameObject grandLv2Model;
        private GameObject grandLv3Model;

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
        private float _alpha;
        private int jumpLevel;

        private bool isCanChangeTwoStep = true;

        private new void Awake()
        {
            base.Awake();
            _events = FindObjectOfType<WeaponEvents>();
            _weaponEnum = WeaponEnum.Grand;

            _sizeLevelValue.Add(GrandSizeLevel.OneGrade, 1f);
            _sizeLevelValue.Add(GrandSizeLevel.TwoGrade, 2f);
            _sizeLevelValue.Add(GrandSizeLevel.FourGrade, 4f);

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;

            chargeBar = GameObject.Find("ChargeBar").transform;

            _sensor = GetComponent<AlphaSensor>();

            _enterCollision = GetComponent<CollisionInteractableObject>();
            _stayCollision = GetComponent<CollisionStayInteractableObject>();

            _dropPoint = transform.GetChild(0);
            _dropLineRenderer = transform.GetChild(1).GetComponent<LineRenderer>();

            _dropPoint.parent = WeaponObjectParentTransform;
            _dropLineRenderer.transform.parent = WeaponObjectParentTransform;

            GetComponent<MeshRenderer>().enabled = false;

            grandLv1Model = transform.GetChild(0).gameObject;
            grandLv2Model = transform.GetChild(1).gameObject;
            grandLv3Model = transform.GetChild(2).gameObject;

            grandLv1Model.SetActive(true);
        }

        private void Start()
        {
            _playerFollow = PlayerBaseTransform.GetComponent<FollowGroundPos>();
            // ���� �÷��̾���� �Ÿ��� alphaSensorValue���� �����ٸ� ������ �ø���.
            _sensor.requirement += () =>
                alphaSensorValue > Vector3.Distance(PlayerBaseTransform.position, transform.position) - _sizeLevelValue[_currentSizeLevel];
        }

        public override void FireWeapon()
        {
            if (Time.time - _fireCoolTime > 0.3f) _fireCoolTime = Time.time;
            else return;

            if (_currentGrandStatus == GrandStatus.Idle)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _dropPoint.gameObject.SetActive(true);
                _dropLineRenderer.gameObject.SetActive(true);

                _alpha = 1;
                Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, 1);

                _fireDir = MainCameraTransform.forward;
                transform.rotation = Quaternion.identity;

                if (Vector3.Angle(_fireDir, -PlayerBaseTransform.up) < 37.5f)
                {
                    _dropPoint.gameObject.SetActive(false);
                    _dropLineRenderer.gameObject.SetActive(false);

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
                        if (dist >= 0.9f)
                            transform.position = FirePosition - (PlayerBaseTransform.up * 0.9f);
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
                    transform.position = FirePosition;

                _currentGrandStatus = GrandStatus.Fire;

                _myCollider.isTrigger = false;
                (_myCollider as BoxCollider).center = Vector3.zero;
            }
        }
        public override void UseWeapon()
        {
            if (_currentGrandStatus == GrandStatus.Idle || _currentGrandStatus == GrandStatus.Resize) return;

            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity /= 10;
            _beforeSizeLevel = _currentSizeLevel;
            _currentGrandStatus = GrandStatus.Use;
        }

        private void ReEnable()
            => _playerFollow.Calculate = true;

        public override void ResetWeapon()
        {
            if (_currentGrandStatus != GrandStatus.Resize && _currentGrandStatus != GrandStatus.Idle)
            {
                _playerFollow.Calculate = false;
                Invoke(nameof(ReEnable), 0.1f); // FIXME: ����
                
                _currentSizeLevel = GrandSizeLevel.OneGrade;
                transform.localScale = Vector3.one;


                chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ?
                                                            0 :
                                                            _sizeLevelValue[_currentSizeLevel] * 0.25f, 1, 1);

                transform.position = HandPosition;
                transform.rotation = Quaternion.identity;
                _myRigid.angularVelocity = Vector3.zero;
                _currentGrandStatus = GrandStatus.Idle;
                _weaponUsedTime = 0f;
                _myRigid.constraints = RigidbodyConstraints.None;


                grandLv1Model.SetActive(true);
                grandLv2Model.SetActive(false);
                grandLv3Model.SetActive(false);

                (_myCollider as BoxCollider).size = Vector3.one * _sizeLevelValue[_currentSizeLevel];

                Update();
            }
        }

        public override bool IsHandleWeapon()
            => _currentGrandStatus == GrandStatus.Idle;

        #region CollisionEvents
        public void BAndATypeObjCollisionEnterEvent(GameObject obj)
        {
            if (_currentGrandStatus != GrandStatus.Resize &&
               _currentGrandStatus != GrandStatus.Use)
            {
                _currentGrandStatus = GrandStatus.LosePower;
            }
        }

        public void BAndATypeObjCollisionStayEvent(GameObject obj)
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
            _enterCollision.isOn = _currentGrandStatus != GrandStatus.Idle;
            _stayCollision.isOn = _currentGrandStatus != GrandStatus.Idle;

            gameObject.layer = _currentGrandStatus == GrandStatus.Idle ? LayerMask.NameToLayer("NOCOLLISION") : LayerMask.NameToLayer("Default");

            switch (_currentGrandStatus)
            {
                case GrandStatus.Idle:

                    if (Vector3.Distance(transform.position, HandPosition) > 2f)
                        transform.position = HandPosition;
                        
                    _myRigid.velocity = (HandPosition - transform.position) * 10;
                    _myRigid.angularVelocity = Vector3.zero;
                    // FIXME: GravityAffectedObject �� Enabled �־�� �װ� �ѹ� ����� -���
                    // ANSWER : �װ� �Ẹ�Ҵµ� �׷� ������ �������������� - To ���
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MainCameraTransform.forward), 0.5f);
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
                            if (isCanChangeTwoStep)
                                MaxSizeLevel();
                            else
                                NextSizeLevel();

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
                    if (_currentLerpTime >= resizeSpeed)
                    {
                        transform.localScale = Vector3.one * _sizeLevelValue[_currentSizeLevel];
                        transform.rotation = Quaternion.identity;
                        _currentGrandStatus = GrandStatus.LosePower;
                        _myRigid.constraints = RigidbodyConstraints.None;

                        _myRigid.angularVelocity = _myRigid.velocity = Vector3.zero;

                        switch (_currentSizeLevel)
                        {
                            case GrandSizeLevel.OneGrade:
                                grandLv1Model.SetActive(true);
                                grandLv2Model.SetActive(false);
                                grandLv3Model.SetActive(false);
                                break;
                            case GrandSizeLevel.TwoGrade:
                                grandLv1Model.SetActive(false);
                                grandLv2Model.SetActive(true);
                                grandLv3Model.SetActive(false);
                                break;
                            case GrandSizeLevel.FourGrade:
                                grandLv1Model.SetActive(false);
                                grandLv2Model.SetActive(false);
                                grandLv3Model.SetActive(true);
                                break;
                        }

                        if(_currentSizeLevel == GrandSizeLevel.OneGrade)
                        {
                            _events?.ChangedMinSize?.Invoke();
                        }
                        else
                        {
                            switch (jumpLevel)
                            {
                                case 1:
                                    _events?.ChangedOneStep?.Invoke();
                                    break;
                                case 2:
                                    _events?.ChangedTwoStep?.Invoke();
                                    break;
                            }
                        }

                        transform.localScale = Vector3.one;
                        (_myCollider as BoxCollider).size = Vector3.one * _sizeLevelValue[_currentSizeLevel];
                    }
                    else
                    {
                        _currentLerpTime += Time.deltaTime;
                        //transform.localScale = Vector3.one * Mathf.Lerp(_beforeWeaponSize, _sizeLevelValue[_currentSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                        transform.localScale = Vector3.one * Mathf.Lerp(1, _sizeLevelValue[_currentSizeLevel] / _sizeLevelValue[_beforeSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
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
            jumpLevel = 0;

            if (_weaponUsedTime >= 0.65f && isCanChangeTwoStep)
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
            _currentSizeLevel = ((_currentSizeLevel == GrandSizeLevel.FourGrade) ? 
                GrandSizeLevel.OneGrade : GrandSizeLevel.FourGrade);
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

                if ((plusHit.transform.CompareTag("BTYPEOBJECT") || plusHit.transform.CompareTag("ATYPEOBJECT")) 
                && ((minusHit.transform.CompareTag("BTYPEOBJECT") || minusHit.transform.CompareTag("ATYPEOBJECT"))))
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

                if (minusAxisDist > curSize + padding)
                    afterPos += -checkDir * padding; // padding ��ŭ �̵�
            }
            else if (minusAxisDist < curSize)
            {
                float padding = curSize - minusAxisDist;

                if (plusAxisDist > curSize + padding)
                    afterPos += checkDir * padding; // padding ��ŭ �̵�
            }
        }
        private float GetDistance(Vector3 dir)
        {
            if (Physics.BoxCast(transform.position, Vector3.one * (_sizeLevelValue[_beforeSizeLevel] / 2 - _sizeLevelValue[_beforeSizeLevel] / 10), dir, out RaycastHit hit))
            {
                if ((hit.transform.CompareTag("BTYPEOBJECT") || hit.transform.CompareTag("ATYPEOBJECT")) && !hit.collider.isTrigger)
                    return Vector3.Distance(hit.point, transform.position);
            }

            return float.MaxValue;
        }

        private void ShowDropPoint()
        {
            if(_currentGrandStatus != GrandStatus.Idle)
            {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, GravityManager.GetGlobalGravityDirection());
                float minDistance = float.MaxValue;
                int index = -1;

                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if ((hits[i].transform.CompareTag("BTYPEOBJECT") || hits[i].transform.CompareTag("ATYPEOBJECT")) && hits[i].distance < minDistance)
                        {
                            minDistance = hits[i].distance;
                            index = i;
                        }
                    }

                    if (index != -1)
                    {
                        RaycastHit hit = hits[index];
                        
                        _dropPoint.position = hit.point + -GravityManager.GetGlobalGravityDirection() * 0.1f;
                        _dropPoint.rotation = Quaternion.LookRotation(-GravityManager.GetGlobalGravityDirection()) * Quaternion.Euler(90, 0, 0);
                        _dropLineRenderer.transform.position = hit.point + -GravityManager.GetGlobalGravityDirection() * (Vector3.Distance(hit.point, transform.position) / 2);

                        _dropLineRenderer.SetPosition(0, -GravityManager.GetGlobalGravityDirection() * (Vector3.Distance(_dropLineRenderer.transform.position, hit.point) - _sizeLevelValue[_currentSizeLevel] / 2));
                        _dropLineRenderer.SetPosition(1, GravityManager.GetGlobalGravityDirection() * Vector3.Distance(_dropLineRenderer.transform.position, hit.point));
                    }
                }

                _dropPoint.gameObject.SetActive(hits != null);
                _dropLineRenderer.gameObject.SetActive(hits != null);
            }

            if (!(_currentGrandStatus == GrandStatus.Fire))
            {
                if (_alpha >= 0.2f)
                {
                    _alpha -= Time.deltaTime * alphaToZeroSpeed;
                    Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                    _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, _alpha > 0.2f ? _alpha : 0);
                }
                else
                    _dropPoint.gameObject.SetActive(false);
                    
                _dropLineRenderer.gameObject.SetActive(false);
            }
        }
    }
}