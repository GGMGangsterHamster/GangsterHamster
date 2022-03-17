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
    private Transform cameraParent;

    [SerializeField]
    private float gravityCameraMoveSpeed = 1f;

    private Rigidbody _myRigid;
    private Collider _myCol;
    private MeshRenderer _myRenderer;
    private Vector3 normalVec = Vector3.zero;
    private float timer = 0f;

    private bool isReadyedChangedGravity = false;
    private bool isChangedGravity = false;

    private IEnumerator shotingCoroutine;

    private void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myCol = GetComponent<Collider>();
        _myRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// ��Ŭ���� ��� ��� ���ư���
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent != null)
        {
            if (IntegratedWeaponSkill.Instance.CheckForward())
            {
                _myRigid.constraints = RigidbodyConstraints.None;

                shotingCoroutine = ShotCo(dir);

                StartCoroutine(shotingCoroutine);
            }
        }
    }

    /// <summary>
    /// RŰ Ŭ���� ���Ⱑ ���ƿȿ� ���ÿ� �߷��� ������� ���ƿ´�.
    /// </summary>
    public void Comeback()
    {
        if (transform.parent != RightHandTrm)
        {
            if (shotingCoroutine != null)
            {
                StopCoroutine(shotingCoroutine);
            }

            _myRigid.useGravity = false;
            _myRigid.velocity = Vector3.zero;
            transform.parent = RightHandTrm;
            transform.localPosition = Vector3.zero;


            _myRigid.constraints = RigidbodyConstraints.FreezePosition;
            //_myCol.isTrigger = true;

            if(isChangedGravity)
            {
                isReadyedChangedGravity = false;
                isChangedGravity = false;
                ResetGravity();
            }
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

        transform.position = PlayerBaseTrm.position + (PlayerBaseTrm.forward / 3) + PlayerBaseTrm.up * PlayerTrm.localScale.y;

        while (true)
        {
            transform.position += dir * shotSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public void ChangeGravity()
    {
        if (normalVec == Vector3.up || !isReadyedChangedGravity || isChangedGravity) return;

        isChangedGravity = true;
        SetGravity();
    }

    /// <summary>
    /// ���𰡿� �ε����� �� ������Ʈ�� ���������� �ݴ�� �߷��� �ٲ�� �ȴ�. �׸��� ����� �����Ѵ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Interactable I) && transform.parent != RightHandTrm && !isReadyedChangedGravity)
        {
            normalVec = collision.contacts[0].normal;

            if (shotingCoroutine != null)
            {
                StopCoroutine(shotingCoroutine);
            }
            // ���⼭ ���⸦ ���߰� �ؾ� ��
            _myRigid.velocity = Vector3.zero;
            _myRigid.useGravity = false;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            isReadyedChangedGravity = true;
        }
    }

    private void SetGravity()
    {
        TransformCheckPointManagement.Instance.SetCheckpoint("BeforeCheckpoint", PlayerBaseTrm);
        TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").position += new Vector3(0, 1.8f, 0);
        // ���� �ٶ󺸰�
        PlayerBaseTrm.rotation = Quaternion.LookRotation(-normalVec);
        PlayerBaseTrm.rotation = Quaternion.LookRotation(PlayerBaseTrm.up);

        if (normalVec.x != 0)
        {
            PlayerBaseTrm.rotation = Quaternion.Euler(new Vector3(PlayerBaseTrm.rotation.eulerAngles.x,
                                                                  PlayerBaseTrm.rotation.eulerAngles.y,
                                                                  normalVec.x < 0 ? 90 : 270));
        }
        else if (normalVec.y != 0)
        {
            PlayerBaseTrm.rotation = Quaternion.Euler(new Vector3(PlayerBaseTrm.rotation.eulerAngles.x,
                                                                  PlayerBaseTrm.rotation.eulerAngles.y,
                                                                  180));
        }
        else if (normalVec.z != 0)
        {
            PlayerBaseTrm.rotation = Quaternion.Euler(new Vector3(PlayerBaseTrm.rotation.eulerAngles.x,
                                                                  PlayerBaseTrm.rotation.eulerAngles.y,
                                                                  normalVec.z < 0 ? 0 : 180));
        }

        GravityManager.Instance.ChangeGlobalGravityDirection(-normalVec);
        TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerBaseTrm);

        _myRenderer.enabled = false;

        CoroutineManagement.Instance.StartCoroutine(CameraRotationAndMoveCoroutine());
    }

    private void ResetGravity()
    {
        if (normalVec == Vector3.zero || normalVec == Vector3.up) return;
        else if(normalVec == Vector3.down)
        {
            PlayerBaseTrm.position += new Vector3(0, -1.8f, 0);
        }

        TransformCheckPointManagement.Instance.SetCheckpoint("BeforeCheckpoint", PlayerBaseTrm);
        GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);

        normalVec = Vector3.zero;

        //float beforeAngle = PlayerTrm.rotation.eulerAngles.x == 0 ? PlayerTrm.rotation.eulerAngles.y : -PlayerTrm.rotation.eulerAngles.x;

        PlayerBaseTrm.rotation = Quaternion.Euler(new Vector3(0, Vector3.Angle(Vector3.forward, PlayerBaseTrm.forward), 0));

        TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerBaseTrm);
        TransformCheckPointManagement.Instance.GetCheckPoint("AfterCheckpoint").position += PlayerBaseTrm.up * 1.8f;
        _myRenderer.enabled = false;

        CoroutineManagement.Instance.StartCoroutine(CameraRotationAndMoveCoroutine());
    }

    IEnumerator CameraRotationAndMoveCoroutine()
    {
        MainCamTrm.parent = cameraParent;
        timer = 0f;

        while (true)
        {
            timer += Time.deltaTime * gravityCameraMoveSpeed;

            MainCamTrm.position = Vector3.Lerp(TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").position,
                                               camPosTrm.position,
                                               timer);

            Quaternion beforeCamRot = TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").rotation;
            TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerBaseTrm);
            Quaternion afterCamRot = TransformCheckPointManagement.Instance.GetCheckPoint("AfterCheckpoint").rotation;

            cameraParent.rotation = Quaternion.Lerp(beforeCamRot, afterCamRot, timer);

            if (timer >= 1f)
            {
                MainCamTrm.parent = camParent;
                MainCamTrm.localPosition = Vector3.zero;
                timer = 0;
                _myRenderer.enabled = true;

                yield break;
            }

            yield return null;
        }
    }
}
