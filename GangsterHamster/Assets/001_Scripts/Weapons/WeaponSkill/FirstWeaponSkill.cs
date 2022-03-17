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
    public float comeBackTime = 0f; // 돌아올때 누적되는 시간

    private WeaponManagement wm;

    private Rigidbody _myRigid; // 무기의 Rigidbody
    private Collider _myCol; // 무기의 Collider
    private Transform objsParent; // 무기에게 붙은 AtypeObj들의 부모 오브젝트

    private Interactable clingObj; // 무기에 붙어있는 오브젝트들을 모아놓은 리스트


    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();
        wm = PlayerBaseTrm.GetComponent<WeaponManagement>();

        objsParent = transform.GetChild(0);
    }

    /// <summary>
    /// 좌클릭 시 무기가 특정 obj에 닿을 때까지 날아감
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
    /// R키를 누를 시 무기가 바로 회수 됨
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
    /// 오른손에 무기가 있지 않을 때
    /// 우클릭을 누를 시 무기가 플레이어 쪽으로 오게 됨
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
    /// 충돌시 정해진 이벤트 발생
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != RightHandTrm)
        {
            if (collision.transform.TryGetComponent(out Interactable outII)) // 만약 TypeObj이라면
            {
                if (clingObj != null && (outII as ObjectA) != null) return; // 이미 뭔가 붙어 있다면 패스

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
            else if (collision.transform.CompareTag("PLAYER_BASE")) // 플레이어라면 오른손에 무기가 돌아오게 한다
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
    /// dir로 계속해서 나아가는 코루틴
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
    /// 플레이어 방향으로 무기를 이동시키는 코루틴
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
                    // 넉백 주는 곳
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
    /// objList를 초기화
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
