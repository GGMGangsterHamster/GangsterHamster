using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondWeaponSkill : MonoBehaviour
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
    private Image chargeBarImg;

    private Rigidbody _myRigid; // 무기의 Rigidbody

    private Dictionary<ScaleEnum, float> scaleDict = new Dictionary<ScaleEnum, float>();

    private ScaleEnum curScaleEnum = ScaleEnum.LevelOne;

    private IEnumerator shotingCoroutine;

    private float curPushTime = 0.0f;

    private bool isEnd = true;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();

        scaleDict.Add(ScaleEnum.LevelOne, 0.5f);
        scaleDict.Add(ScaleEnum.LevelTwo, 1f);
        scaleDict.Add(ScaleEnum.LevelThree, 2f);
        scaleDict.Add(ScaleEnum.LevelFour, 4f);
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
        if(curScaleEnum == ScaleEnum.LevelOne && isEnd)
        {
            if (transform.parent == null)
            {
                StopAllCoroutines();

                _myRigid.useGravity = false;
                _myRigid.velocity = Vector3.zero;

                transform.parent = rightHandTrm;
                transform.localPosition = Vector3.zero;

                isEnd = true;
            }
        }
    }

    public void ScaleUp()
    {
        if (isEnd)
        {
            isEnd = false;

            if (curScaleEnum == ScaleEnum.LevelFour)
            {
                curScaleEnum = ScaleEnum.LevelOne;
            }
            else
            {
                curScaleEnum++;
            }

            SetScale(curScaleEnum);
        }
    }

    private void SetScale(ScaleEnum scaleEnum)
    {
        _myRigid.useGravity = true;

        Debug.Log(scaleEnum);
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

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

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

        // 누르고 있는 시간 체크용 반복문
        while(true)
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

        bool test = scaleDistance >= 0 ? true : false;

        while (true)
        {
            transform.localScale += Vector3.one * Time.deltaTime * scaleDistance / 0.5f;

            if (test)
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
}
