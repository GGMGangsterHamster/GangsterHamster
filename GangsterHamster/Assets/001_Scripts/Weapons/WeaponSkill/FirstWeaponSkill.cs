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

    private Rigidbody _myRigid; // 무기의 Rigidbody
    private Collider _myCol; // 무기의 Collider
    private Transform objsParent; // 무기에게 붙은 AtypeObj들의 부모 오브젝트

    private List<Interactable> objList; // 무기에 붙어있는 오브젝트들을 모아놓은 리스트

    private Vector3 moveVec = Vector3.zero; // 지금 무기가 움직이고 있는 방향

    private float comeBackTime = 0f; // 돌아올때 누적되는 시간

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();

        objsParent = transform.GetChild(0);
        objList = new List<Interactable>();

        wm = PlayerTrm.GetComponent<WeaponManagement>();
    }

    /// <summary>
    /// 1번 항목에 대한 함수
    /// 오른손에 무기가 있을 때
    /// 좌클릭 시 무기가 특정 obj에 닿을 때까지 날아감
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if(transform.parent == RightHandTrm)
        {
            if (lastFireTime + delay <= Time.time) {
                Vector3 boxSize = PlayerTrm.GetComponent<BoxCollider>().size;

                Collider[] cols = Physics.OverlapBox(PlayerTrm.position + (PlayerTrm.forward / 2) + new Vector3(0, boxSize.y / 2, 0),
                                                     boxSize + new Vector3(0, -1f, 0.3f), 
                                                     PlayerTrm.rotation); // 플레이어의 바로 앞을 검사해서 뭔가 있는지 확인

                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].TryGetComponent(out Interactable outII)) // 만약 상호작용 되는 오브젝트가 앞에 있었으면 리턴
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
    /// 2번 항목에 대한 함수
    /// 오른손에 무기가 있지 않을 때
    /// R키를 누를 시 무기가 바로 회수 됨
    /// </summary>
    /// <param name="rightHandTrm"> 플레이어 오른손의 위치를 받아오기 위한 Trm </param>
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
    /// 3, 4번 항목에 대한 함수
    /// 오른손에 무기가 있지 않을 때
    /// 우클릭을 누를 시 무기가 플레이어 쪽으로 오게 됨
    /// </summary>
    /// <param name="distTrm"> 플레이어의 Trm </param>
    /// <param name="waitFollowTime"> 우클릭을 몇초동안 얼마나 눌러야 플레이어를 계속 따라오는가 </param>
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
                    if ((outII as ObjectB) != null) // Type이 B일 경우
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
    /// 5번 항목에 대한 함수 
    /// 오른손에 무기가 있지 않을 때
    /// A, B 타입의 오브젝트에 충돌시 정해진 이벤트 발생
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // 만약 이미 무기가 가지고 있는 오브젝트 일 경우는 패스
        for (int i = 0; i < objsParent.childCount; i++)
        {
            if(collision.transform == objsParent.GetChild(i))
            {
                return;
            }
        }

        if (transform.parent != RightHandTrm)
        {
            if (collision.transform.TryGetComponent(out Interactable outII)) // 만약 TypeObj이라면
            {
                StopAllCoroutines();

                outII.Collision(objsParent.gameObject);

                if ((outII as ObjectA) != null) // Type이 A일 경우 무기의 중력을 없애고 움직임을 얼린다
                {
                    _myRigid.velocity = Vector3.zero;
                    _myRigid.useGravity = false;
                    _myCol.isTrigger = true;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    objList.Add(outII);
                }
                else if ((outII as ObjectB) != null) // Type이 B일 경우 붙어있던 모든 오브젝트를 떼어내고 중력을 활성화하며 움직임도 활성화 한다
                {
                    ClearList();

                    _myRigid.velocity = moveVec / shotSpeed;
                    _myRigid.useGravity = true;
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            else if (collision.transform.CompareTag("PLAYER_BASE")) // 플레이어라면 오른손에 무기가 돌아오게 한다
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
    /// dir로 계속해서 나아가는 코루틴
    /// </summary>
    /// <param name="dir"> 방향 </param>
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
    /// 플레이어 방향으로 무기를 이동시키는 코루틴
    /// waitFollowTime 만큼 계속해서 누르고 있으면 이후에는 멈추지 않고 플레이어를 따라감
    /// </summary>
    /// <param name="distTrm"> 무기가 가야하는 목적지의 Trm </param>
    /// <param name="rightHandTrm"> 플레이어 오른손 위치의 Trm </param>
    /// <param name="waitFollowTime"> 몇초 동안 우클릭을 눌러야 하는지 </param>
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
                if (Input.GetKey(KeyCode.Mouse1)) // 1.5초동안 우클릭을 누르고 있다면 플레이어를 지속적으로 따라감
                {
                    waitFollowTime -= Time.deltaTime;

                    if (waitFollowTime <= 0)
                    {
                        isFollow = true;
                        endFollowCheck = true;
                    }
                }
                else if(Input.GetKeyUp(KeyCode.Mouse1)) // 중간에 우클릭을 떼어버리면 가던 방향으로 계속 간다
                {
                    endFollowCheck = true;
                }
            }

            if(isFollow)
            {
                dir = (distTrm.position - transform.position).normalized;
            }

            transform.position += dir * Time.deltaTime * shotSpeed;

            if (Vector3.Distance(distTrm.position, transform.position) <= knockbackSize) // 플레이어와 거리가 1 이하라면 오른손으로 돌아오게 한다
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
    /// objList를 초기화
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
