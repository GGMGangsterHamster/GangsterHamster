using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Commands.Weapon;
using static Define;

public class FirstWeaponSkill : WeaponSkill
{
    [SerializeField]
    private float shotSpeed = 5;

    [SerializeField]
    private float knockbackSize = 1.7f;

    private WeaponManagement wm;

    private Rigidbody _myRigid; // ������ Rigidbody
    private Collider _myCol; // ������ Collider
    private Transform objsParent; // ���⿡�� ���� AtypeObj���� �θ� ������Ʈ

    private List<Interactable> objList; // ���⿡ �پ��ִ� ������Ʈ���� ��Ƴ��� ����Ʈ

    private Vector3 moveVec = Vector3.zero; // ���� ���Ⱑ �����̰� �ִ� ����

    private float comeBackTime = 0f; // ���ƿö� �����Ǵ� �ð�

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();

        objsParent = transform.GetChild(0);
        objList = new List<Interactable>();

        wm = PlayerTrm.GetComponent<WeaponManagement>();
    }

    /// <summary>
    /// 1�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ��
    /// ��Ŭ�� �� ���Ⱑ Ư�� obj�� ���� ������ ���ư�
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if(transform.parent == RightHandTrm)
        {
            if (lastFireTime + delay <= Time.time) {
                Vector3 boxSize = PlayerTrm.GetComponent<BoxCollider>().size;

                Collider[] cols = Physics.OverlapBox(PlayerTrm.position + (PlayerTrm.forward / 2) + new Vector3(0, boxSize.y / 2, 0),
                                                     boxSize + new Vector3(0, -1f, 0.3f), 
                                                     PlayerTrm.rotation); // �÷��̾��� �ٷ� ���� �˻��ؼ� ���� �ִ��� Ȯ��

                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].TryGetComponent(out Interactable outII)) // ���� ��ȣ�ۿ� �Ǵ� ������Ʈ�� �տ� �־����� ����
                    {
                        return;
                    }
                }

                StartCoroutine(ShotCo(dir));
                lastFireTime = Time.time;
            }
        }
    }
    float lastFireTime = float.MinValue;
    public float delay = 0.5f;

    /// <summary>
    /// 2�� �׸� ���� �Լ�
    /// �����տ� ���Ⱑ ���� ���� ��
    /// RŰ�� ���� �� ���Ⱑ �ٷ� ȸ�� ��
    /// </summary>
    /// <param name="rightHandTrm"> �÷��̾� �������� ��ġ�� �޾ƿ��� ���� Trm </param>
    public void ComeBack(Transform rightHandTrm)
    {
        if(transform.parent != RightHandTrm)
        {
            ClearList();

            StopAllCoroutines();

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;

            transform.parent = rightHandTrm;
            transform.localPosition = Vector3.zero;

            comeBackTime = 0f;
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
            Collider[] cols;
            Vector3 dir = (distTrm.position - transform.position).normalized;

            cols = Physics.OverlapSphere(transform.position + dir * Time.deltaTime * shotSpeed, transform.localScale.x * 0.45f);

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent(out Interactable outII))
                {
                    if ((outII as ObjectB) != null) // Type�� B�� ���
                    {
                        return;
                    }
                }
            }

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector2.zero;

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

        if (transform.parent != RightHandTrm)
        {
            if (collision.transform.TryGetComponent(out Interactable outII)) // ���� TypeObj�̶��
            {
                StopAllCoroutines();

                outII.Collision(objsParent.gameObject);

                if ((outII as ObjectA) != null) // Type�� A�� ��� ������ �߷��� ���ְ� �������� �󸰴�
                {
                    _myRigid.velocity = Vector3.zero;
                    _myRigid.useGravity = false;
                    _myCol.isTrigger = true;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    objList.Add(outII);
                }
                else if ((outII as ObjectB) != null) // Type�� B�� ��� �پ��ִ� ��� ������Ʈ�� ����� �߷��� Ȱ��ȭ�ϸ� �����ӵ� Ȱ��ȭ �Ѵ�
                {
                    ClearList();

                    _myRigid.velocity = moveVec / shotSpeed;
                    _myRigid.useGravity = true;
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            else if (collision.transform.CompareTag("PLAYER_BASE")) // �÷��̾��� �����տ� ���Ⱑ ���ƿ��� �Ѵ�
            { 
                if(wm.lastWeaponNumber == 0)
                {
                    wm.SetMaxWeaponNumber(1);
                }

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

        transform.position = PlayerTrm.position + PlayerTrm.forward + new Vector3(0, 1.8f, 0);

        while (true)
        {
            moveVec = dir * shotSpeed;
            
            transform.position += moveVec * Time.deltaTime;

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
            comeBackTime += Time.deltaTime;
            moveVec = dir * shotSpeed;

            if (!endFollowCheck)
            {
                if (Input.GetKey(KeyCode.Mouse1)) // 1.5�ʵ��� ��Ŭ���� ������ �ִٸ� �÷��̾ ���������� ����
                {
                    waitFollowTime -= Time.deltaTime;

                    if (waitFollowTime <= 0)
                    {
                        isFollow = true;
                        endFollowCheck = true;
                    }
                }
                else if(Input.GetKeyUp(KeyCode.Mouse1)) // �߰��� ��Ŭ���� ��������� ���� �������� ��� ����
                {
                    endFollowCheck = true;
                }
            }

            if(isFollow)
            {
                dir = (distTrm.position - transform.position).normalized;
            }

            transform.position += dir * Time.deltaTime * shotSpeed;

            if (Vector3.Distance(distTrm.position, transform.position) <= knockbackSize) // �÷��̾�� �Ÿ��� 1 ���϶�� ���������� ���ƿ��� �Ѵ�
            {
                if (objsParent.childCount > 0)
                {
                    PlayerTrm.GetComponent<Rigidbody>().velocity = moveVec * comeBackTime;
                }
                
                comeBackTime = 0f;
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
        foreach(Interactable obj in objList)
        {
            obj.Release();
        }

        objList.Clear();
    }

#if UNITY_EDITOR

#endif
}
