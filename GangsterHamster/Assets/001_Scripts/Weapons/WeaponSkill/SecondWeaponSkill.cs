using Objects.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Commands.Weapon;

public class SecondWeaponSkill : WeaponSkill
{
    enum ScaleEnum
    {
        LevelOne,
        LevelTwo,
        LevelThree,
        LevelFour,
    }

    [SerializeField]
    private float shotSpeed = 5;

    [SerializeField]
    private float sizeUpSpeed = 0.1f;

    [SerializeField]
    private float idleRotationMaxSpeed = 4f;

    [SerializeField]
    private Image chargeBarImg;

    private WeaponManagement wm;

    private Rigidbody _myRigid; // 무기의 Rigidbody

    private Dictionary<ScaleEnum, float> scaleDict = new Dictionary<ScaleEnum, float>();

    private ScaleEnum curScaleEnum = ScaleEnum.LevelOne;
    private ScaleEnum beforeScaleEnum = ScaleEnum.LevelOne;

    private IEnumerator shotingCoroutine;
    private IEnumerator rotationCoroutine;

    private float curPushTime = 0.0f;

    private bool isEnd = true;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();

        scaleDict.Add(ScaleEnum.LevelOne, 0.5f);
        scaleDict.Add(ScaleEnum.LevelTwo, 1f);
        scaleDict.Add(ScaleEnum.LevelThree, 2f);
        scaleDict.Add(ScaleEnum.LevelFour, 4f);

        wm = GameObject.Find("Player").GetComponent<WeaponManagement>();
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
            shotingCoroutine = ShotCo(dir);

            StartCoroutine(shotingCoroutine);
        }
    }

    /// <summary>
    /// 2번 항목에 대한 함수
    /// 오른손에 무기가 있지 않을 때
    /// R키를 누를 시 무기가 바로 회수 됨
    /// </summary>
    /// <param name="rightHandTrm"> 플레이어 오른손의 위치를 받아오기 위한 Trm </param>
    public void ComeBack(Transform rightHandTrm)
    {
        if(isEnd)
        {
            if (transform.parent == null)
            {
                StopAllCoroutines();

                _myRigid.useGravity = false;
                _myRigid.velocity = Vector3.zero;
                _myRigid.constraints = RigidbodyConstraints.None;
                rotationCoroutine = RotationCo();
                StartCoroutine(rotationCoroutine);

                transform.parent = rightHandTrm;
                transform.localPosition = Vector3.zero;

                curScaleEnum = ScaleEnum.LevelOne;

                curPushTime = 0f;
                chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);
                transform.localScale = Vector3.one * scaleDict[curScaleEnum];

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
            if (transform.parent == null)
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

    /// <summary>
    /// dir로 계속해서 나아가는 코루틴
    /// </summary>
    /// <param name="dir"> 방향 </param>
    /// <returns></returns>
    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;
        StopCoroutine(rotationCoroutine);

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

            yield return null;
        }
    }

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
            StopCoroutine(shotingCoroutine);
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
        #region
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
        #endregion

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
        if (collision.transform.CompareTag("PLAYER_BASE") && wm.lastWeaponNumber == 1) // 플레이어라면 오른손에 무기가 돌아오게 한다
        {
            wm.SetMaxWeaponNumber(2);

            ComeBack(GameObject.Find("RightHand").transform);
        }
    }
}

// -- 긴 코드 --