using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Gravity.Object.Management;

public class ThirdWeaponSkill : WeaponSkill
{
    // �ϴ��� BoxCollider�� �������� �Ѵ� ���� Sphere�� ���� ������ ���ϱ� �ָ��� ���� ���Ⱑ �ٽ� ���ƿ��� �Ѵ�.

    [SerializeField]
    private float shotSpeed = 5;

    private WeaponManagement wm;
    private IEnumerator shotingCoroutine;
    private Rigidbody _myRigid;
    private Transform playerTrm; // �÷��̾��� Trm

    private Collision col = null;

    private bool isChangedGravity = false;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();

        playerTrm = GameObject.Find("Player").transform;
        wm = playerTrm.GetComponent<WeaponManagement>();
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
            // �ε��� �� ������Ʈ�� �鿡�� ���� �������� �߷��� �ٲ۴� 
            // ���� �̹� �ٲ��� �ִ� ���¶�� �׳� �ƹ��͵� ���ϰ� �ѱ��.
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
