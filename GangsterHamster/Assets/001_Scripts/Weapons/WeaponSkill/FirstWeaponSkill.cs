using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Commands.Weapon;
using static Define;

public class FirstWeaponSkill : WeaponSkill
{
    public float shotSpeed = 5;
    public float knockbackDistance = 1.7f;
    public float knockbackPower = 1f;

    [HideInInspector]
    public float comeBackTime = 0f; // ���ƿö� �����Ǵ� �ð�

    private WeaponManagement wm;

    private Rigidbody _myRigid; // ������ Rigidbody
    private Collider _myCol; // ������ Collider
    private Transform objsParent; // ���⿡�� ���� AtypeObj���� �θ� ������Ʈ

    private Interactable clingObj; // ���⿡ �پ��ִ� ������Ʈ���� ��Ƴ��� ����Ʈ


    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();
        wm = PlayerBaseTrm.GetComponent<WeaponManagement>();

        objsParent = transform.GetChild(0);
    }

    /// <summary>
    /// ��Ŭ�� �� ���Ⱑ Ư�� obj�� ���� ������ ���ư�
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent == RightHandTrm)
        {
            if (IntegratedWeaponSkill.Instance.CheckForward(PlayerBaseTrm.forward))
            {
                StartCoroutine(ShotCo(dir));
            }
        }
    }

    /// <summary>
    /// RŰ�� ���� �� ���Ⱑ �ٷ� ȸ�� ��
    /// </summary>
    public void ComeBack()
    {
        if(transform.parent != RightHandTrm)
        {
            ClearObj();
            StopAllCoroutines();

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;
            transform.parent = RightHandTrm;
            transform.localPosition = Vector3.zero;
            comeBackTime = 0f;
        }
    }

    /// <summary>
    /// �����տ� ���Ⱑ ���� ���� ��
    /// ��Ŭ���� ���� �� ���Ⱑ �÷��̾� ������ ���� ��
    /// </summary>
    public void MoveToDestination()
    {
        if(transform.parent == null)
        {
            Vector3 dir = (MainCamTrm.position - transform.position).normalized;
            Collider[] cols = Physics.OverlapSphere(transform.position + dir * Time.deltaTime * shotSpeed, transform.localScale.x * 0.45f);

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent(out Interactable outII))
                {
                    if ((outII as ObjectB) != null)
                    {
                        return;
                    }
                }
            }

            _myRigid.useGravity = false;
            StopAllCoroutines();
            StartCoroutine(MoveToDistCo());
        }
    }

    /// <summary>
    /// �浹�� ������ �̺�Ʈ �߻�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != RightHandTrm)
        {
            if (collision.transform.TryGetComponent(out Interactable outII)) // ���� TypeObj�̶��
            {
                if (clingObj != null && (outII as ObjectA) != null) return; // �̹� ���� �پ� �ִٸ� �н�

                StopAllCoroutines();
                outII.Collision(objsParent.gameObject);

                if ((outII as ObjectA) != null)
                {
                    clingObj = outII;

                    ObjectACollision(collision);
                }
                else if ((outII as ObjectB) != null)
                {
                    ObjectBCollision(collision);
                }
            }
            else if (collision.transform.CompareTag("PLAYER_BASE")) // �÷��̾��� �����տ� ���Ⱑ ���ƿ��� �Ѵ�
            { 
                if(wm.lastWeaponNumber == 0)
                {
                    wm.SetMaxWeaponNumber(1);
                }

                ComeBack();
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
    /// <returns></returns>
    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;
        transform.position = PlayerBaseTrm.position + (PlayerBaseTrm.forward / 3) + PlayerBaseTrm.up * PlayerTrm.localScale.y;

        while (true)
        {            
            transform.position += dir * shotSpeed * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// �÷��̾� �������� ���⸦ �̵���Ű�� �ڷ�ƾ
    /// </summary>
    IEnumerator MoveToDistCo()
    {
        Vector3 moveDir;

        while(true)
        {
            comeBackTime += Time.deltaTime;
            moveDir = (MainCamTrm.position - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * shotSpeed;

            if (Vector3.Distance(MainCamTrm.position, transform.position) <= knockbackDistance)
            {
                if (objsParent.childCount > 0)
                {
                    // �˹� �ִ� ��
                    PlayerBaseTrm.GetComponent<Rigidbody>().velocity = moveDir * knockbackPower * comeBackTime;
                }
                
                comeBackTime = 0f;
                ComeBack();

                yield break;
            }


            yield return null;
        }
    }

    public void ObjectACollision(Collision collision)
    {
        _myRigid.useGravity = false;
        _myCol.isTrigger = true;

        _myRigid.constraints = RigidbodyConstraints.FreezeAll;
        collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ObjectBCollision(Collision collision)
    {
        ClearObj();

        _myRigid.useGravity = true;
        _myRigid.velocity = collision.contacts[0].normal / shotSpeed;
        _myRigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// objList�� �ʱ�ȭ
    /// </summary>
    private void ClearObj()
    {
        if(clingObj != null)
        {
            clingObj.Release();
            clingObj = null;
        }
    }
}
