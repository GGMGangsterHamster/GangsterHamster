using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Gravity.Object.Management;
using Player.Movement;
using static Define;

public class ThirdWeaponSkill : WeaponSkill
{
    // �ϴ��� BoxCollider�� �������� �Ѵ� ���� Sphere�� ���� ������ ���ϱ� �ָ��� ���� ���Ⱑ �ٽ� ���ƿ��� �Ѵ�.

    [SerializeField]
    private float shotSpeed = 5;
    [SerializeField]
    private Transform CamPosTrm;

    private WeaponManagement wm;
    private Rigidbody _myRigid;
    private Collider _myCol;
    private Transform playerTrm; // �÷��̾��� Trm
    private PlayerMovement _movement;
    private Vector3 normalVec = Vector3.zero;

    private Vector3 beforeCamPos = Vector3.zero;
    private Vector3 beforeCamRot = Vector3.zero;
    private float afterCamRotY = 0f;
    private float gravityLerpSpeed = 0f;
    private float timer = 0f;

    private bool isChangedGravity = false;
    private bool isMoveAfterPos = false;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();

        playerTrm = Define.PlayerTrm;
        wm = playerTrm.GetComponent<WeaponManagement>();
        _movement = playerTrm.GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// ��Ŭ���� ��� ��� ���ư���
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent != null)
        {
            StartCoroutine(ShotCo(dir));
        }
    }

    /// <summary>
    /// RŰ Ŭ���� ���Ⱑ ���ƿȿ� ���ÿ� �߷��� ������� ���ƿ´�.
    /// </summary>
    /// <param name="rightHandTrm"></param>
    public void Comeback(Transform rightHandTrm)
    {
        if (transform.parent == null)
        {
            StopAllCoroutines();

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;
            _myRigid.constraints = RigidbodyConstraints.None;
            _myCol.isTrigger = true;

            transform.parent = rightHandTrm;
            transform.localPosition = Vector3.zero;
            isChangedGravity = false;

            ChangeGravity();
        }
    }

    /// <summary>
    /// ����ؼ� ������ ���ư��� �ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="dir">���ư��� ����</param>
    /// <returns></returns>
    IEnumerator ShotCo(Vector3 dir)
    {
        transform.parent = null;
        _myCol.isTrigger = false;

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// ���𰡿� �ε����� �� ������Ʈ�� ���������� �ݴ�� �߷��� �ٲ�� �ȴ�. �׸��� ����� �����Ѵ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Interactable I) && transform.parent != wm.transform && !isChangedGravity)
        {
            isChangedGravity = true;
            // �ε��� �� ������Ʈ�� �鿡�� ���� �������� �߷��� �ٲ۴� 
            // ���� �̹� �ٲ��� �ִ� ���¶�� �׳� �ƹ��͵� ���ϰ� �ѱ��.
            normalVec = collision.contacts[0].normal;
            float angle = Vector3.Angle(playerTrm.up, collision.contacts[0].normal);

            GravityManager.Instance.ChangeGlobalGravityDirection(-normalVec);
            playerTrm.rotation = Quaternion.Euler(new Vector3(normalVec.z != 0 ? normalVec.z > 0 ? angle : -angle : normalVec.y != 0 ? normalVec.y > 0 ? 0 : angle * 2 : 0
                                                            , 0
                                                            , normalVec.x != 0 ? normalVec.z > 0 ? -angle : angle : 0
            ));
            // ���⼭ ���⸦ ���߰� �ؾ� ��
            _myRigid.velocity = Vector3.zero;
            _myRigid.useGravity = false;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void ChangeGravity()
    {
        beforeCamPos = MainCamTrm.position;
        beforeCamRot = MainCamTrm.rotation.eulerAngles;

        GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);

        MainCamTrm.parent = null;

        afterCamRotY = MainCamTrm.rotation.eulerAngles.x;

        isMoveAfterPos = true;
        timer = 0;

        gravityLerpSpeed = Vector3.Distance(beforeCamPos, CamPosTrm.position);

        float beforeAngle = playerTrm.rotation.eulerAngles.x == 0 ? playerTrm.rotation.eulerAngles.y : -playerTrm.rotation.eulerAngles.x;
        playerTrm.rotation = Quaternion.Euler(new Vector3(0, beforeAngle, 0));
    }

    private void Update()
    {
        Debug.Log("Before : " + (MainCamTrm.rotation.eulerAngles.x >= 270 ? (MainCamTrm.rotation.eulerAngles.x - 360) : MainCamTrm.rotation.eulerAngles.x));
        if (isMoveAfterPos)
        {
            timer += Time.deltaTime;
            Debug.Log("After : " + afterCamRotY);

            MainCamTrm.position = Vector3.Lerp(beforeCamPos, CamPosTrm.position, timer);
            _movement.OnMouseY(-Mathf.Lerp((beforeCamRot.x >= 270 ? (beforeCamRot.x - 360) : beforeCamRot.x), afterCamRotY, timer));

            if(timer >= 1f)
            {
                MainCamTrm.parent = CamPosTrm;
                MainCamTrm.localPosition = Vector3.zero;
                _movement.OnMouseY(afterCamRotY);

                isMoveAfterPos = false;
                timer = 0;
            }
        }
    }
}
