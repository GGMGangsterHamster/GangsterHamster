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
    /// 1번 항목에 대한 함수
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
    /// R키를 누를 시 무기가 바로 회수 됨
    /// </summary>
    /// <param name="rightHandTrm"> 플레이어 오른손의 위치를 받아오기 위한 Trm </param>
    public void ComeBack(Transform rightHandTrm)
    {
        if(transform.parent == null)
        {
            for (int i = 0; i < objsParent.childCount; i++)
            {
                Transform childTrm = objsParent.GetChild(i);
                // 무기가 가지고 있던 오브젝트들을 다 떼어낸다

                // 나중에 이 코드 바꿀꺼임 더러운 코드
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
    /// 3, 4번 항목에 대한 함수
    /// 우클릭을 누를 시 무기가 플레이어 쪽으로 오게 됨
    /// </summary>
    /// <param name="distTrm"> 플레이어의 Trm </param>
    /// <param name="waitFollowTime"> 우클릭을 몇초동안 얼마나 눌러야 플레이어를 계속 따라오는가 </param>
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
    /// 5번 항목에 대한 함수 
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

        // 만약 오른손에 무기가 있을 경우는 패스
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

                // 무기가 붙는다!
            }
            else if (collision.gameObject.CompareTag("B_Obj"))
            {
                StopAllCoroutines();

                for (int i = 0; i < objsParent.childCount; i++)
                {
                    Transform childTrm = objsParent.GetChild(i);
                    // 무기가 가지고 있던 오브젝트들을 다 떼어낸다

                    // 나중에 이 코드 바꿀꺼임 더러운 코드
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
                // 무기가 떨어진다!
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _col.isTrigger = false;
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
}
