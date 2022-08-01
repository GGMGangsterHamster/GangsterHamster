using Characters.Player;
using Characters.Player.Move;
using Matters.Gravity;
using Matters.Velocity;
using Objects;
using Objects.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions.Broker;

// 기능 코드는 100줄 남짓
// 버그 코드는 400줄 이상

namespace Weapons.Actions
{        
    // 그랜드의 크기 변환 단계
    public enum GrandSizeLevel
    {
        OneGrade,
        TwoGrade,
        FourGrade,
    }

    [RequireComponent(typeof(GravityAffectedObject))]
    public class Grand : WeaponAction
    {
        public string WeaponKeyCodePath = "KeyCodes/Weapons.json";
        public float resizeSpeed; // 크기 변환할 때 드는 시간
        public float chargeSpeed; // 차징 하는 속도
        public float reboundPower;
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
        
        // WeaponEvents Singleton 패턴 피하기 위함
        private WeaponEvents _events;

        private FollowGroundPos _myFollowGroundPos;
        private FollowGroundPos _playerFollowGroundPos;

        private AllWeaponMessageBroker _AWmessageBroker;
        private GrandMessageBroker _messageBroker;

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

        private GameObject grandLv1Model;
        private GameObject grandLv2Model;
        private GameObject grandLv3Model;

        private Transform _cain;
        private Transform _cain_nucleus;

        public Dictionary<GrandSizeLevel, float> _sizeLevelValue = new Dictionary<GrandSizeLevel, float>();

        [HideInInspector] public GrandSizeLevel _currentSizeLevel = GrandSizeLevel.OneGrade;
        [HideInInspector] public GrandSizeLevel _beforeSizeLevel = GrandSizeLevel.OneGrade;
        [HideInInspector] public GrandStatus _currentGrandStatus = GrandStatus.Idle;

        private Quaternion lerpQuaternion;
        private KeyCode _useKeycode;

        private float _beforeWeaponSize = 0f;
        private float _currentLerpTime = 0f;
        private float _weaponUsedTime = 0f;

        private float _fireCoolTime;
        private float _alpha;
        private int jumpLevel;

        private bool isCanChangeTwoStep = true;

        private new void Awake()
        {
            base.Awake();
            _events = FindObjectOfType<WeaponEvents>();
            _weaponEnum = WeaponEnum.Grand;
            _myRigid.constraints = RigidbodyConstraints.FreezePosition;

            _sizeLevelValue.Add(GrandSizeLevel.OneGrade, 1f);
            _sizeLevelValue.Add(GrandSizeLevel.TwoGrade, 2f);
            _sizeLevelValue.Add(GrandSizeLevel.FourGrade, 4f);

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(WeaponKeyCodePath);
            _useKeycode = (KeyCode)vo.Use;

            _enterCollision = GetComponent<CollisionInteractableObject>();
            _stayCollision = GetComponent<CollisionStayInteractableObject>();

            _dropPoint = transform.GetChild(0);
            _dropLineRenderer = transform.GetChild(1).GetComponent<LineRenderer>();

            _dropPoint.parent = WeaponObjectParentTransform;
            _dropLineRenderer.transform.parent = WeaponObjectParentTransform;

            chargeBar = GameObject.Find("ChargeBar").transform;

            _myFollowGroundPos = GetComponent<FollowGroundPos>();
            _playerFollowGroundPos = PlayerBaseTransform.GetComponent<FollowGroundPos>();

            _messageBroker = GetComponent<GrandMessageBroker>();
            _AWmessageBroker = transform.parent.GetComponent<AllWeaponMessageBroker>();

            grandLv1Model = transform.GetChild(0).gameObject;
            grandLv2Model = transform.GetChild(1).gameObject;
            grandLv3Model = transform.GetChild(2).gameObject;

            grandLv1Model.SetActive(true);
        }

        public override void FireWeapon()
        {
            if (Time.time - _fireCoolTime > 0.3f) _fireCoolTime = Time.time;
            else return;

            if (_currentGrandStatus == GrandStatus.Idle)
            {
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;

                _AWmessageBroker.OnFire?.Invoke();
                _messageBroker.OnFire?.Invoke();

                _myRigid.velocity = Vector3.zero;
                _myRigid.angularVelocity = Vector3.zero;

                _dropPoint.gameObject.SetActive(true);
                _dropLineRenderer.gameObject.SetActive(true);
                
                _alpha = 1;
                Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, 1);

                _fireDir = MainCameraTransform.forward;
                transform.rotation = Quaternion.identity;

                _currentGrandStatus = GrandStatus.Fire;

                if (Vector3.Angle(_fireDir, -PlayerBaseTransform.up) < 37.5f)
                {
                    _dropPoint.gameObject.SetActive(false);
                    _dropLineRenderer.gameObject.SetActive(false);

                    bool b = Physics.Raycast(PlayerBaseTransform.position + (PlayerBaseTransform.up * 0.01f), _fireDir, out RaycastHit hit);
                    float dist = Vector3.Distance(PlayerBaseTransform.position, hit.point);

                    if (b && dist < 0.2f)
                    {
                        transform.position = FirePosition + (PlayerStatus.IsCrouching ? hit.normal : Vector3.zero);
                        PlayerBaseTransform.position += PlayerBaseTransform.up * (1.2f - (PlayerStatus.IsCrouching ? 0 : dist));
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
                {
                    transform.position = FirePosition;
                }

                _myCollider.isTrigger = false;
                (_myCollider as BoxCollider).center = Vector3.zero;
            }
        }
        public override void UseWeapon()
        {
            if (_currentGrandStatus == GrandStatus.Idle || _currentGrandStatus == GrandStatus.Resize) return;

            if(_currentSizeLevel == GrandSizeLevel.OneGrade)
                _myRigid.velocity = Vector3.zero;

            _AWmessageBroker.OnUse?.Invoke();
            _messageBroker.OnUse?.Invoke();

            _beforeSizeLevel = _currentSizeLevel;
            _currentGrandStatus = GrandStatus.Use;
        }

        public override void ResetWeapon()
        {
            if (_currentGrandStatus != GrandStatus.Resize && _currentGrandStatus != GrandStatus.Idle)
            {
                _AWmessageBroker.OnReset?.Invoke();
                _messageBroker.OnReset?.Invoke();
                
                _playerFollowGroundPos.Deactive(gameObject);
                _myFollowGroundPos.Active(null);

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
                _myRigid.velocity = Vector3.zero;
                _myRigid.angularVelocity = Vector3.zero;
            }
        }

        public void BAndATypeObjCollisionStayEvent(GameObject obj)
        {
            if (_currentGrandStatus != GrandStatus.Resize &&
                _currentGrandStatus != GrandStatus.Use &&
                _currentGrandStatus != GrandStatus.LosePower)
            {
                _currentGrandStatus = GrandStatus.LosePower;
                _myRigid.velocity = Vector3.zero;
                _myRigid.angularVelocity = Vector3.zero;
            }
        }
        #endregion

        private void Update()
        {
            _enterCollision.Activated = _currentGrandStatus != GrandStatus.Idle;
            _stayCollision.isOn = _currentGrandStatus != GrandStatus.Idle;

            gameObject.layer = _currentGrandStatus == GrandStatus.Idle ? LayerMask.NameToLayer("NOCOLLISION") : LayerMask.NameToLayer("Default");
            
            switch (_currentGrandStatus)
            {
                case GrandStatus.Idle:

                    transform.position = HandPosition;
                    transform.rotation = Quaternion.Slerp(transform.rotation, MainCameraTransform.rotation, 0.5f);
                    break;

                case GrandStatus.Fire:
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;

                case GrandStatus.Use:
                    if (Input.GetKey(_useKeycode))
                    {
                        _weaponUsedTime += Time.deltaTime * chargeSpeed;
                        chargeBar.localScale = new Vector3(ChargeBarValue, 1, 1);
                        // 차징 되는 UI 보여주기

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
                        // 키를 누르고 떼면 Resize로 이동
                    }
                    break;

                case GrandStatus.Resize: // 크기 변환 과정
                    if (_currentLerpTime >= resizeSpeed)
                    {
                        transform.localScale = Vector3.one * _sizeLevelValue[_currentSizeLevel];
                        _cain.localScale = Vector3.one;
                        _cain_nucleus.localScale = Vector3.one;
                        transform.rotation = Quaternion.identity;
                        _currentGrandStatus = GrandStatus.LosePower;
                        _myRigid.constraints = RigidbodyConstraints.None;

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
                        transform.localScale = Vector3.one * Mathf.Lerp(1, _sizeLevelValue[_currentSizeLevel] / _sizeLevelValue[_beforeSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                        transform.rotation = Quaternion.Lerp(transform.rotation, lerpQuaternion, Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));

                        _cain.localScale = Vector3.one / Mathf.Lerp(1, _sizeLevelValue[_currentSizeLevel] / _sizeLevelValue[_beforeSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
                        _cain_nucleus.localScale = Vector3.one / Mathf.Lerp(1, _sizeLevelValue[_currentSizeLevel] / _sizeLevelValue[_beforeSizeLevel], Mathf.Clamp(_currentLerpTime / resizeSpeed, 0, 0.99f));
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
            // 여기서 정해진 조건에 충족하지 못하는 경우 밑의 코드를 실행하지 못함
            if (!CanResize(Vector3.up) ||
               !CanResize(Vector3.right) ||
               !CanResize(Vector3.forward))
            {
                return;
            }

            // 크기 변환 전 초기 작업
            float x, y, z;

            _currentGrandStatus = GrandStatus.Resize;
            _weaponUsedTime = 0f;
            _currentLerpTime = 0f;
            _beforeWeaponSize = transform.localScale.x;
            chargeBar.localScale = new Vector3(_currentSizeLevel == GrandSizeLevel.OneGrade ? 0 : _sizeLevelValue[_currentSizeLevel] * 0.25f
                                                , 1, 1);

            // 이런 저런 조건에 맞으면 플레이어에게 반동을 주고 데미지도 줌
            // 1. 작아지는 경우라면 실행 안함
            // 2. 플레이어가 정해진 범위에 들어와야 실행함
            if (_sizeLevelValue[_currentSizeLevel] - _beforeWeaponSize > 0)
            {
                if (((_sizeLevelValue[_currentSizeLevel] + (_currentSizeLevel == GrandSizeLevel.OneGrade ? 1 : 0)) / 3) > Vector3.Distance(transform.position, PlayerBaseTransform.position) - (PlayerBaseTransform.localScale.x + PlayerBaseTransform.localScale.y) / 3)
                {
                    Vector3 reboundDir = (PlayerBaseTransform.position - transform.position).normalized;
                    float rebound = (_sizeLevelValue[_currentSizeLevel] - _sizeLevelValue[_beforeSizeLevel]) * reboundPower + 2;

                    float maxValue = Mathf.Max(Mathf.Abs(reboundDir.x),
                                     Mathf.Max(Mathf.Abs(reboundDir.y),
                                               Mathf.Abs(reboundDir.z)));

                    x = maxValue == Mathf.Abs(reboundDir.x) ? rebound * Mathf.Sign(reboundDir.x) - (Vector3.Distance(transform.position, PlayerBaseTransform.position)) : 0;
                    y = maxValue == Mathf.Abs(reboundDir.y) ? rebound * Mathf.Sign(reboundDir.y) - (Vector3.Distance(transform.position, PlayerBaseTransform.position)) : 0;
                    z = maxValue == Mathf.Abs(reboundDir.z) ? rebound * Mathf.Sign(reboundDir.z) - (Vector3.Distance(transform.position, PlayerBaseTransform.position)) : 0;

                    PlayerBaseTransform.GetComponent<Rigidbody>().velocity = new Vector3(x, y, z); // 도형의 각도를 무시하고 World 좌표로 반동 주는거

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

            if(grandLv1Model.activeSelf)
            {
                _cain = grandLv1Model.transform.Find("cain");
                _cain_nucleus = grandLv1Model.transform.Find("cain_nucleus");
            }
            else if(grandLv2Model.activeSelf)
            {
                _cain = grandLv2Model.transform.Find("cain");
                _cain_nucleus = grandLv2Model.transform.Find("cain_nucleus");
            }
            else
            {
                _cain = grandLv3Model.transform.Find("cain");
                _cain_nucleus = grandLv3Model.transform.Find("cain_nucleus");
            }
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
            if (Physics.Raycast(transform.position, checkDir, out RaycastHit plusHit, _sizeLevelValue[_currentSizeLevel]) &&
                Physics.Raycast(transform.position, -checkDir, out RaycastHit minusHit, _sizeLevelValue[_currentSizeLevel]))
            {

                if ((plusHit.transform.CompareTag("BTYPEOBJECT") || plusHit.transform.CompareTag("ATYPEOBJECT")) 
                && (minusHit.transform.CompareTag("BTYPEOBJECT") || minusHit.transform.CompareTag("ATYPEOBJECT"))
                && !plusHit.collider.isTrigger && !minusHit.collider.isTrigger)
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
                        _dropPoint.rotation = Quaternion.LookRotation(-GravityManager.GetGlobalGravityDirection());
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