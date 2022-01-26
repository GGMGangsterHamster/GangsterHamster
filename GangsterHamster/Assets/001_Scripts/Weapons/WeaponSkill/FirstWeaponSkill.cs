using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

public class FirstWeaponSkill : MonoBehaviour
{
    [SerializeField]
    private float shotSpeed = 5;

    private Rigidbody _myRigid; // 무기의 Rigidbody
    private Collider _myCol; // 무기의 Collider
    private Transform objsParent; // 무기에게 붙은 AtypeObj들의 부모 오브젝트

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
    /// 1번 항목에 대한 함수
    /// 오른손에 무기가 있을 때
    /// 좌클릭 시 무기가 특정 obj에 닿을 때까지 날아감
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if(transform.parent != null)
        {
            StartCoroutine(ShotCo(dir));
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
            _myRigid.useGravity = false;

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

        if (transform.parent == null)
        {
            if(collision.transform.TryGetComponent(out IInteractableObject outII))
            {
                StopAllCoroutines();

                outII.Collision(objsParent.gameObject);

                _myRigid.velocity = Vector3.zero;

                if((outII as ObjectA) != null) // Type이 A일 경우
                {
                    _myRigid.useGravity = false;
                    _myCol.isTrigger = true;

                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    objList.Add(outII);
                }
                else if((outII as ObjectB) != null) // Type이 B일 경우
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
    /// dir로 계속해서 나아가는 코루틴
    /// </summary>
    /// <param name="dir"> 방향 </param>
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
    /// objList를 초기화
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
