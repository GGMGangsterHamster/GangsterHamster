using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using Gravity.Object.Management;
using Player.Movement;
using static Define;
using Player.Mouse;

public class ThirdWeaponSkill : WeaponSkill
{
    // �ϴ��� BoxCollider�� �������� �Ѵ� ���� Sphere�� ���� ������ ���ϱ� �ָ��� ���� ���Ⱑ �ٽ� ���ƿ��� �Ѵ�.

    [SerializeField]
    private float shotSpeed = 5;
    [SerializeField]
    private Transform camPosTrm;
    [SerializeField]
    private Transform camParent;
    [SerializeField]
    private Transform tempCameraParent;

    [SerializeField]
    private float gravityCameraMoveSpeed = 1f;

    private WeaponManagement wm;
    private Rigidbody _myRigid;
    private Collider _myCol;
    private MeshRenderer _myRenderer;
    private Transform playerTrm; // �÷��̾��� Trm
    private Vector3 normalVec = Vector3.zero;
    private float timer = 0f;

    private bool isChangedGravity = false;
    private bool isMoveAfterPos = false;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();
        _myRenderer = GetComponent<MeshRenderer>();

        playerTrm = Define.PlayerTrm;
        wm = playerTrm.GetComponent<WeaponManagement>();
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

            ResetGravity();
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

    private void SetGravity(Vector3 gravityDir)
    {

    }

    private void ResetGravity()
    {
        if (normalVec == Vector3.zero || normalVec == Vector3.down) return;

        normalVec = Vector3.zero;

        TransformCheckPointManagement.Instance.SetCheckpoint("BeforeCheckpoint", PlayerTrm);
        GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);

        MainCamTrm.parent = tempCameraParent;
        MainCamTrm.rotation = TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").rotation;
        isMoveAfterPos = true;
        timer = 0;

        float beforeAngle = playerTrm.rotation.eulerAngles.x == 0 ? playerTrm.rotation.eulerAngles.y : -playerTrm.rotation.eulerAngles.x;
        playerTrm.rotation = Quaternion.Euler(new Vector3(0, beforeAngle, 0));

        TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerTrm);
        _myRenderer.enabled = false;
    }

    private void Update()
    {
        if (isMoveAfterPos)
        {
            timer += Time.deltaTime * gravityCameraMoveSpeed;

            MainCamTrm.position = Vector3.Lerp(TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").position, 
                                               camPosTrm.position, 
                                               timer);

            Quaternion beforeCamRot = TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").rotation;
            TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerTrm);
            Quaternion afterCamRot = TransformCheckPointManagement.Instance.GetCheckPoint("AfterCheckpoint").rotation;

            // �� �� : ���� ������ �ƴ϶� - ������ �ϰ� �ֱ⶧���� ����̰� Set�Լ��� ������ָ� �װŸ� ����Ұ���
            //_movement.SetMouseY(-Mathf.Lerp(beforeCamRotY, 
            //                                afterCamRotY, 
            //                                timer));

            tempCameraParent.rotation = Quaternion.Lerp(beforeCamRot, afterCamRot, timer);

            if(timer >= 1f)
            {
                MainCamTrm.parent = camParent;
                MainCamTrm.localPosition = Vector3.zero;

                isMoveAfterPos = false;
                timer = 0;
                _myRenderer.enabled = true;
            }
        }
    }
}
