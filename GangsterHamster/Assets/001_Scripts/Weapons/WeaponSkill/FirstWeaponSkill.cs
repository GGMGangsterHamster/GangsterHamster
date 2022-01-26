using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

public class FirstWeaponSkill : MonoBehaviour
{
    [SerializeField]
    private float shotSpeed = 5;

    private Rigidbody _myRigid; // ������ Rigidbody
    private Collider _myCol; // ������ Collider
    private Transform objsParent; // ���⿡�� ���� AtypeObj���� �θ� ������Ʈ

    private List<IInteractableObject> objList;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();

        objsParent = transform.GetChild(0);
        objList = new List<IInteractableObject>();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 1�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ��
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
    /// �����տ� ���Ⱑ ���� ���� ��
    /// RŰ�� ���� �� ���Ⱑ �ٷ� ȸ�� ��
    /// </summary>
    /// <param name="rightHandTrm"> �÷��̾� �������� ��ġ�� �޾ƿ��� ���� Trm </param>
    public void ComeBack(Transform rightHandTrm)
    {
        if(transform.parent == null)
        {
            ClearList();

            StopAllCoroutines();

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;

            transform.parent = rightHandTrm;
            transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 3, 4�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ���� ��
    /// ��Ŭ���� ���� �� ���Ⱑ �÷��̾� ������ ���� ��
    /// </summary>
    /// <param name="distTrm"> �÷��̾��� Trm </param>
    /// <param name="waitFollowTime"> ��Ŭ���� ���ʵ��� �󸶳� ������ �÷��̾ ��� ������°� </param>
    public void MoveToDestination(Transform distTrm, Transform rightHandTrm, float waitFollowTime)
    {
        if(transform.parent == null)
        {
            _myRigid.useGravity = false;

            StopAllCoroutines();

            StartCoroutine(MoveToDistCo(distTrm, rightHandTrm, waitFollowTime));
        }
    }

    /// <summary>
    /// 5�� �׸� ���� �Լ� 
    /// �����տ� ���Ⱑ ���� ���� ��
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

        if (transform.parent == null)
        {
            if(collision.transform.TryGetComponent(out IInteractableObject outII))
            {
                StopAllCoroutines();

                outII.Collision(objsParent.gameObject);

                _myRigid.velocity = Vector3.zero;

                if((outII as ObjectA) != null) // Type�� A�� ���
                {
                    _myRigid.useGravity = false;
                    _myCol.isTrigger = true;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    objList.Add(outII);
                }
                else if((outII as ObjectB) != null) // Type�� B�� ���
                {
                    ClearList();

                    _myRigid.useGravity = true;
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            else if(collision.transform.CompareTag("PLAYER_BASE"))
            {
                ComeBack(GameObject.Find("RightHand").transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _myCol.isTrigger = false;
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

    /// <summary>
    /// objList�� �ʱ�ȭ
    /// </summary>
    private void ClearList()
    {
        foreach(IInteractableObject obj in objList)
        {
            obj.Release();
        }

        objList.Clear();
    }
}
