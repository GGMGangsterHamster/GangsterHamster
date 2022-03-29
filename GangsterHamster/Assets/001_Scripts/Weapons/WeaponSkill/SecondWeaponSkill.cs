using Objects.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Commands.Weapon;
using static Define;

public class SecondWeaponSkill : WeaponSkill
{
    enum ScaleEnum // 무기가 커지는 단계를 나타내는 enum
    {
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelFour,
    }

    [SerializeField] private float shotSpeed = 5;
    [SerializeField] private float sizeUpSpeed = 0.1f;
    [SerializeField] private float idleRotationMaxSpeed = 4f;
    [SerializeField] private float rightClickPower = 20f;

    [SerializeField] private Image chargeBarImg;

    private WeaponManagement wm;
    private Rigidbody _myRigid; // 무기의 Rigidbody
    private Collider _myCol;

    private Dictionary<ScaleEnum, float> scaleDict = new Dictionary<ScaleEnum, float>();
    private ScaleEnum curScaleEnum = ScaleEnum.LevelOne;
    private ScaleEnum beforeScaleEnum = ScaleEnum.LevelOne;
    private IEnumerator shotingCoroutine;
    private IEnumerator rotationCoroutine;
    private Vector3 _colNormalVec; 


    private float curPushTime = 0.0f;
    private bool isEnd = true;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();

        scaleDict.Add(ScaleEnum.LevelOne, 0.5f);
        scaleDict.Add(ScaleEnum.LevelTwo, 1f);
        scaleDict.Add(ScaleEnum.LevelThree, 2f);
        scaleDict.Add(ScaleEnum.LevelFour, 4f);

        wm = PlayerBaseTrm.GetComponent<WeaponManagement>();
    }

    /// <summary>
    /// 1번 항목에 대한 함수
    /// 오른손에 무기가 있을 때
    /// 좌클릭 시 무기가 특정 obj에 닿을 때까지 날아감
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent != null)
        {
            if (IntegratedWeaponSkill.Instance.CheckForward(PlayerBaseTrm.forward))
            {
                shotingCoroutine = ShotCo(dir);

                _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                StartCoroutine(shotingCoroutine);
            }
        }
    }

    /// <summary>
    /// 2번 항목에 대한 함수
    /// 오른손에 무기가 있지 않을 때
    /// R키를 누를 시 무기가 바로 회수 됨
    /// </summary>
    public void ComeBack()
    {
        if(isEnd)
        {
            if (transform.parent != RightHandTrm)
            {
                StopAllCoroutines();

                _myRigid.useGravity = false;
                _myRigid.velocity = Vector3.zero;
                _myRigid.constraints = RigidbodyConstraints.FreezePosition;
                //_myCol.isTrigger = true;

                rotationCoroutine = RotationCo();
                StartCoroutine(rotationCoroutine);

                transform.parent = RightHandTrm;
                transform.localPosition = Vector3.zero;

                curScaleEnum = ScaleEnum.LevelOne;
                    
                curPushTime = 0f;
                chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);
                transform.localScale = Vector3.one * scaleDict[ScaleEnum.LevelOne];

                isEnd = true;
            }
        }
    }

    /// <summary>
    /// 우클릭시 실행되는 함수
    /// 크기가 커진다
    /// </summary>
    public void ScaleUp()
    {
        if (isEnd)
        {
            if (transform.parent != RightHandTrm)
            {
                SetScale(curScaleEnum);
            }
        }
    }

    private void SetScale(ScaleEnum scaleEnum)
    {
        isEnd = false;

        beforeScaleEnum = curScaleEnum;

        if (curScaleEnum == ScaleEnum.LevelFour)
        {
            curScaleEnum = ScaleEnum.LevelOne;
        }
        else
        {
            curScaleEnum++;
        }


        _myRigid.useGravity = true;

        StartCoroutine(SetScaleCo(scaleEnum));
    }

    public void StopShotingMove()
    {
        if (shotingCoroutine != null)
        {
            StopCoroutine(shotingCoroutine);

            shotingCoroutine = null;
        }
    }

    /// <summary>
    /// dir로 계속해서 나아가는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;
        _myCol.isTrigger = false;
        StopCoroutine(rotationCoroutine);

        transform.position = PlayerBaseTrm.position + (PlayerBaseTrm.forward / 3) + PlayerBaseTrm.up * PlayerTrm.localScale.y;

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// 계속 반복해서 Vector3(1,1,1) 의 각도로 돌리게 하는 코루틴
    /// 멈추는건 알아서 다른 코드에서 해줌
    /// </summary>
    /// <returns></returns>
    IEnumerator RotationCo()
    {
        while(true)
        {
            if(_myRigid.angularVelocity.x <= idleRotationMaxSpeed)
            {
                _myRigid.AddTorque(Vector3.one * Time.deltaTime);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 정해진 크기까지 정해진 속도로 커지게 하는 코루틴
    /// </summary>
    /// <param name="scaleEnum">다음으로 변해야 하는 상태</param>
    /// <returns></returns>
    IEnumerator SetScaleCo(ScaleEnum scaleEnum)
    {
        if(shotingCoroutine != null)
        {
            StopShotingMove();

            _myRigid.useGravity = true;
            _myRigid.constraints = RigidbodyConstraints.None;
            _myRigid.velocity = _colNormalVec * rightClickPower;
        }


        float beforeCurPushTime = curPushTime;

        // 누르고 있는 시간 체크용 반복문
        while (true)
        {
            if(curPushTime >= 1.5f)
            {
                // 이미 4단계(4m)라면 그냥 넘어감
                curPushTime = 0f;
                chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);
                curScaleEnum = ScaleEnum.LevelOne;
                break;
            }

            if(Input.GetKey(KeyCode.Mouse1))
            {
                curPushTime += Time.deltaTime;

                if (curPushTime >= 1.5f) // 끝까지 눌렀을때는 4단계로 직행
                {
                    chargeBarImg.transform.localScale = new Vector3(1.5f, 1, 1);
                    curScaleEnum = ScaleEnum.LevelFour;

                    break;
                }
                else
                {
                    chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);
                }
            }
            else // 중간에 키를 뗏을때
            {
                if(curPushTime <= 0.5f)
                {
                    // 2단계로
                    curScaleEnum = ScaleEnum.LevelTwo;
                    curPushTime = 0.5f;
                }
                else if(curPushTime <= 1.0f)
                {
                    // 3단계로
                    curScaleEnum = ScaleEnum.LevelThree;
                    curPushTime = 1f;
                }
                else if(curPushTime <= 1.5f)
                {
                    // 4단계로
                    curScaleEnum = ScaleEnum.LevelFour;
                    curPushTime = 1.5f;
                }

                chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);

                break;
            }

            yield return null;
        }

        scaleEnum = curScaleEnum;

        float scaleDistance = scaleDict[scaleEnum] - transform.localScale.x;

        bool comparator = scaleDistance >= 0 ? true : false; // 큰지 작은지 확인하는 bool 변수

        // 주변에 상호작용 할 수 있는 오브젝트가 가까이 있는지 확인해주는 코드 => 코드가 좀 많이 더러움...
        Collider[] cols = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + scaleDict[curScaleEnum] + 0.05f, transform.position.z)
                                            , Vector3.one * scaleDict[curScaleEnum] / 2
                                            , transform.rotation);

        List<Collider> colList = cols.ToList();

        for (int i = 0; i < colList.Count; i++)
        {
            if (colList[i].transform == transform) continue;

            if (colList[i].TryGetComponent(out Interactable outII)) // 만약 상호작용 되는 오브젝트가 주변에 있다면 리턴
            {
                isEnd = true;
                chargeBarImg.transform.localScale = new Vector3(beforeCurPushTime, 1, 1);
                curPushTime = beforeCurPushTime;
                curScaleEnum = beforeScaleEnum;
                yield break;
            }
            else
            {
                colList.RemoveAt(i);
                i--;
            }
        }

        while (true) // 서서히 정해진 크기까지 작아지거나 커지는 반복문
        {
            transform.localScale += Vector3.one * Time.deltaTime * scaleDistance / sizeUpSpeed;

            if (comparator)
            {
                if(transform.localScale.x >= scaleDict[scaleEnum])
                {
                    transform.localScale = Vector3.one * scaleDict[scaleEnum];

                    break;
                }
            }
            else
            {
                if (transform.localScale.x <= scaleDict[scaleEnum])
                {
                    transform.localScale = Vector3.one * scaleDict[scaleEnum];

                    break;
                }
            }

            yield return null;
        }

        isEnd = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out ObjectB outII))
        {
            StopShotingMove();

            _myRigid.velocity = Vector3.zero;
            _myRigid.useGravity = false;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;

            _colNormalVec = collision.contacts[0].normal;
        }
        if (collision.transform.CompareTag("PLAYER_BASE") && wm.lastWeaponNumber == 1) // 플레이어라면 오른손에 무기가 돌아오게 한다
        {
            wm.SetMaxWeaponNumber(2);

            ComeBack();
        }
    }
}

// -- 긴 코드 --