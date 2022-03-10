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

    private Rigidbody _myRigid; // ������ Rigidbody

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
    /// ��Ŭ���� ����Ǵ� �Լ�
    /// ũ�Ⱑ Ŀ����
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
    /// dir�� ����ؼ� ���ư��� �ڷ�ƾ
    /// </summary>
    /// <param name="dir"> ���� </param>
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

        float beforeCurPushTime = curPushTime;

        // ������ �ִ� �ð� üũ�� �ݺ���
        while (true)
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

        bool comparator = scaleDistance >= 0 ? true : false; // ū�� ������ Ȯ���ϴ� bool ����

        // �ֺ��� ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� ������ �ִ��� Ȯ�����ִ� �ڵ� => �ڵ尡 �� ���� ������...
        #region
        Collider[] cols = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + scaleDict[curScaleEnum] + 0.05f, transform.position.z)
                                            , Vector3.one * scaleDict[curScaleEnum] / 2
                                            , transform.rotation);

        List<Collider> colList = cols.ToList();

        for (int i = 0; i < colList.Count; i++)
        {
            if (colList[i].transform == transform) continue;

            if (colList[i].TryGetComponent(out Interactable outII)) // ���� ��ȣ�ۿ� �Ǵ� ������Ʈ�� �ֺ��� �ִٸ� ����
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

        while (true) // ������ ������ ũ����� �۾����ų� Ŀ���� �ݺ���
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
        if (collision.transform.CompareTag("PLAYER_BASE") && wm.lastWeaponNumber == 1) // �÷��̾��� �����տ� ���Ⱑ ���ƿ��� �Ѵ�
        {
            wm.SetMaxWeaponNumber(2);

            ComeBack(GameObject.Find("RightHand").transform);
        }
    }
}

// -- �� �ڵ� --