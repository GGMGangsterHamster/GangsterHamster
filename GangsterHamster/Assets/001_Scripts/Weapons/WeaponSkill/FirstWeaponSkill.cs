using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWeaponSkill : MonoBehaviour
{
    [SerializeField]
    private float shotSpeed = 5;

    private Rigidbody _rigid;

    private Collider _col;

    private Transform objsParent;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        objsParent = transform.GetChild(0);
    }

    private void Update()
    {
        if(transform.parent != null)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 1�� �׸� ���� �Լ�
    /// ��Ŭ�� �� ���Ⱑ Ư�� obj�� ���� ������ ���ư�
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if(transform.parent != null)
        {
            StartCoroutine(ShotCo(dir));
        }
    }

    /// <summary>
    /// 2�� �׸� ���� �Լ�
    /// RŰ�� ���� �� ���Ⱑ �ٷ� ȸ�� ��
    /// </summary>
    /// <param name="rightHandTrm"> �÷��̾� �������� ��ġ�� �޾ƿ��� ���� Trm </param>
    public void ComeBack(Transform rightHandTrm)
    {
        if(transform.parent == null)
        {
            for (int i = 0; i < objsParent.childCount; i++)
            {
                Transform childTrm = objsParent.GetChild(i);
                // ���Ⱑ ������ �ִ� ������Ʈ���� �� �����

                // ���߿� �� �ڵ� �ٲܲ��� ������ �ڵ�
                if (childTrm.CompareTag("A_Obj"))
                {
                    objsParent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }

                childTrm.parent = null;

                i--;
            }
            
            _rigid.useGravity = false;
            _rigid.velocity = Vector3.zero;

            StopAllCoroutines();

            transform.parent = rightHandTrm;
            transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 3, 4�� �׸� ���� �Լ�
    /// ��Ŭ���� ���� �� ���Ⱑ �÷��̾� ������ ���� ��
    /// </summary>
    /// <param name="distTrm"> �÷��̾��� Trm </param>
    /// <param name="waitFollowTime"> ��Ŭ���� ���ʵ��� �󸶳� ������ �÷��̾ ��� ������°� </param>
    public void MoveToDestination(Transform distTrm, Transform rightHandTrm, float waitFollowTime)
    {
        if(transform.parent == null)
        {
            _rigid.useGravity = false;

            StopAllCoroutines();

            StartCoroutine(MoveToDistCo(distTrm, rightHandTrm, waitFollowTime));
        }
    }

    /// <summary>
    /// 5�� �׸� ���� �Լ� 
    /// A, B Ÿ���� ������Ʈ�� �浹�� ������ �̺�Ʈ �߻�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // ���� �̹� ���Ⱑ ������ �ִ� ������Ʈ �� ���� �н�
        for (int i = 0; i < objsParent.childCount; i++)
        {
            if(collision.transform == objsParent.GetChild(i))
            {
                return;
            }
        }

        // ���� �����տ� ���Ⱑ ���� ���� �н�
        if (transform.parent == null)
        {
            if (collision.gameObject.CompareTag("A_Obj"))
            {
                StopAllCoroutines();

                _rigid.useGravity = false;
                _rigid.velocity = Vector3.zero;

                _col.isTrigger = true;

                collision.transform.parent = objsParent;

                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                // ���Ⱑ �ٴ´�!
            }
            else if (collision.gameObject.CompareTag("B_Obj"))
            {
                StopAllCoroutines();

                for (int i = 0; i < objsParent.childCount; i++)
                {
                    Transform childTrm = objsParent.GetChild(i);
                    // ���Ⱑ ������ �ִ� ������Ʈ���� �� �����

                    // ���߿� �� �ڵ� �ٲܲ��� ������ �ڵ�
                    if (childTrm.CompareTag("A_Obj"))
                    {
                        objsParent.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }

                    childTrm.parent = null;

                    i--;
                }

                _rigid.useGravity = true;
                _rigid.velocity = Vector3.zero;

                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                // ���Ⱑ ��������!
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _col.isTrigger = false;
    }

    /// <summary>
    /// dir�� ����ؼ� ���ư��� �ڷ�ƾ
    /// </summary>
    /// <param name="dir"> ���� </param>
    /// <returns></returns>
    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;

        while(true)
        {
            transform.position += dir * Time.deltaTime * shotSpeed;

            yield return null;
        }
    }

    /// <summary>
    /// �÷��̾� �������� ���⸦ �̵���Ű�� �ڷ�ƾ
    /// waitFollowTime ��ŭ ����ؼ� ������ ������ ���Ŀ��� ������ �ʰ� �÷��̾ ����
    /// </summary>
    /// <param name="distTrm"> ���Ⱑ �����ϴ� �������� Trm </param>
    /// <param name="rightHandTrm"> �÷��̾� ������ ��ġ�� Trm </param>
    /// <param name="waitFollowTime"> ���� ���� ��Ŭ���� ������ �ϴ��� </param>
    /// <returns></returns>
    IEnumerator MoveToDistCo(Transform distTrm, Transform rightHandTrm, float waitFollowTime)
    {
        Vector3 dir = (distTrm.position - transform.position).normalized;

        bool isFollow = false;
        bool endFollowCheck = false;

        while(true)
        {
            if(!endFollowCheck)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    waitFollowTime -= Time.deltaTime;

                    if (waitFollowTime <= 0)
                    {
                        isFollow = true;
                        endFollowCheck = true;
                    }
                }
                else
                {
                    endFollowCheck = true;
                }
            }

            if(isFollow)
            {
                dir = (distTrm.position - transform.position).normalized;
            }

            transform.position += dir * Time.deltaTime * shotSpeed;

            if(Vector3.Distance(distTrm.position, transform.position) <= 1f)
            {
                ComeBack(rightHandTrm);
                yield break;
            }

            yield return null;
        }
    }
}
