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

    private Rigidbody _myRigid; // ������ Rigidbody

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
    /// 1�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ��
    /// ��Ŭ�� �� ���Ⱑ Ư�� obj�� ���� ������ ���ư�
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
    /// 2�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ���� ��
    /// RŰ�� ���� �� ���Ⱑ �ٷ� ȸ�� ��
    /// </summary>
    /// <param name="rightHandTrm"> �÷��̾� �������� ��ġ�� �޾ƿ��� ���� Trm </param>
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
    /// dir�� ����ؼ� ���ư��� �ڷ�ƾ
    /// </summary>
    /// <param name="dir"> ���� </param>
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
    /// ������ ũ����� ������ �ӵ��� Ŀ���� �ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="scaleEnum">�������� ���ؾ� �ϴ� ����</param>
    /// <returns></returns>
    IEnumerator SetScaleCo(ScaleEnum scaleEnum)
    {
        if(shotingCoroutine != null)
        {
            StopCoroutine(shotingCoroutine);
        }

        // ������ �ִ� �ð� üũ�� �ݺ���
        while(true)
        {
            if(curPushTime >= 1.5f)
            {
                // �̹� 4�ܰ�(4m)��� �׳� �Ѿ
                curPushTime = 0f;
                chargeBarImg.transform.localScale = new Vector3(curPushTime, 1, 1);
                curScaleEnum = ScaleEnum.LevelOne;
                break;
            }

            if(Input.GetKey(KeyCode.Mouse1))
            {
                curPushTime += Time.deltaTime;

                if (curPushTime >= 1.5f) // ������ ���������� 4�ܰ�� ����
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
            else // �߰��� Ű�� ������
            {
                if(curPushTime <= 0.5f)
                {
                    // 2�ܰ��
                    curScaleEnum = ScaleEnum.LevelTwo;
                    curPushTime = 0.5f;
                }
                else if(curPushTime <= 1.0f)
                {
                    // 3�ܰ��
                    curScaleEnum = ScaleEnum.LevelThree;
                    curPushTime = 1f;
                }
                else if(curPushTime <= 1.5f)
                {
                    // 4�ܰ��
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
