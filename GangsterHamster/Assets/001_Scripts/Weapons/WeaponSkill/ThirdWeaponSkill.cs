using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Gravity.Object.Management;

public class ThirdWeaponSkill : WeaponSkill
{
    // 일단은 BoxCollider를 기준으로 한다 만약 Sphere와 같이 각도를 구하기 애매한 것은 무기가 다시 돌아오게 한다.

    [SerializeField]
    private float shotSpeed = 5;

    private WeaponManagement wm;
    private IEnumerator shotingCoroutine;
    private Rigidbody _myRigid;
    private Transform playerTrm; // 플레이어의 Trm

    private Collision col = null;

    private bool isChangedGravity = false;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();

        playerTrm = GameObject.Find("Player").transform;
        wm = playerTrm.GetComponent<WeaponManagement>();
    }

    /// <summary>
    /// 1번 항목에 대한 함수
    /// 오른손에 무기가 있을 때
    /// 좌클릭 시 무기가 특정 obj에 닿을 때까지 날아감
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent != null)
        {
            StartCoroutine(ShotCo(dir));
        }
    }

    public void Comeback(Transform rightHandTrm)
    {
        if (transform.parent == null)
        {
            StopAllCoroutines();

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.None;

            transform.parent = rightHandTrm;
            transform.localPosition = Vector3.zero;
            isChangedGravity = false;
            GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);
            playerTrm.rotation = Quaternion.Euler(0, playerTrm.rotation.y, 0);
        }
    }

    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Interactable I) && transform.parent != wm.transform && !isChangedGravity)
        {
            // 부딪힌 그 오브젝트의 면에서 수직 방향으로 중력을 바꾼다 
            // 만약 이미 바꿔져 있는 상태라면 그냥 아무것도 안하고 넘긴다.
            col = collision;
            Vector3 normal = collision.contacts[0].normal;
            float angle = Vector3.Angle(playerTrm.up, collision.contacts[0].normal);

            GravityManager.Instance.ChangeGlobalGravityDirection(-normal);
            playerTrm.rotation = Quaternion.Euler(new Vector3(normal.z != 0 ? normal.z > 0 ? angle : -angle : 0
                                                            , 0
                                                            , normal.x != 0 ? normal.z > 0 ? -angle : angle : 0
            ));
            Debug.Log(Vector3.Angle(playerTrm.up, collision.contacts[0].normal));
            Debug.Log(collision.contacts[0].normal);

            isChangedGravity = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(col.contacts[0].point, col.contacts[0].normal);
    }
}
