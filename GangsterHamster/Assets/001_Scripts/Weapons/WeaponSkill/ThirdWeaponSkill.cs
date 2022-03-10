using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

public class ThirdWeaponSkill : WeaponSkill
{
    // �ϴ��� BoxCollider�� �������� �Ѵ� ���� Sphere�� ���� ������ ���ϱ� �ָ��� ���� ���Ⱑ �ٽ� ���ƿ��� �Ѵ�.

    [SerializeField]
    private float shotSpeed = 5;

    private WeaponManagement wm;
    private IEnumerator shotingCoroutine;
    private Rigidbody _myRigid;

    private Collision col = null;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();

        wm = GameObject.Find("Player").transform.GetComponent<WeaponManagement>();
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
        if(collision.transform.TryGetComponent(out Interactable I) && transform.parent != wm.transform)
        {
            // �ε��� �� ������Ʈ�� �鿡�� ���� �������� �߷��� �ٲ۴� 
            // ���� �̹� �ٲ��� �ִ� ���¶�� �׳� �ƹ��͵� ���ϰ� �ѱ��.
            col = collision;
            //collision.contacts[0].
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(col.contacts[0].point, col.contacts[0].normal);
    }
}
