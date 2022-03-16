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
    // 일단은 BoxCollider를 기준으로 한다 만약 Sphere와 같이 각도를 구하기 애매한 것은 무기가 다시 돌아오게 한다.

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

    private WeaponManagement wm;
    private Rigidbody _myRigid;
    private Collider _myCol;
    private MeshRenderer _myRenderer;
    private Transform playerTrm; // 플레이어의 Trm
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
    /// 좌클릭시 계속 계속 날아간다
    /// </summary>
    public void Shot(Vector3 dir)
    {
        if (transform.parent != null)
        {
            StartCoroutine(ShotCo(dir));
        }
    }

    /// <summary>
    /// R키 클릭시 무기가 돌아옴에 동시에 중력이 원래대로 돌아온다.
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
    /// 계속해서 앞으로 나아가게 하는 코루틴
    /// </summary>
    /// <param name="dir">나아가는 방향</param>
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
    /// 무언가에 부딪히면 그 오브젝트의 수직벡터의 반대로 중력이 바뀌게 된다. 그리고 무기는 정지한다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Interactable I) && transform.parent != wm.transform && !isChangedGravity)
        {
            TransformCheckPointManagement.Instance.SetCheckpoint("BeforeCheckpoint", PlayerTrm);

            TransformCheckPointManagement.Instance.GetCheckPoint("BeforeCheckpoint").position += new Vector3(0, 1.8f, 0);
            isChangedGravity = true;
            // 부딪힌 그 오브젝트의 면에서 수직 방향으로 중력을 바꾼다 
            // 만약 이미 바꿔져 있는 상태라면 그냥 아무것도 안하고 넘긴다.
            normalVec = collision.contacts[0].normal;

            // 위를 바라보게
            playerTrm.rotation = Quaternion.LookRotation(-normalVec);
            playerTrm.rotation = Quaternion.LookRotation(playerTrm.up);

            // 여기서 무기를 멈추게 해야 함
            _myRigid.velocity = Vector3.zero;
            _myRigid.useGravity = false;
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;

            SetGravity(normalVec);
        }
    }

    private void SetGravity(Vector3 gravityNormalDir)
    {
        GravityManager.Instance.ChangeGlobalGravityDirection(-gravityNormalDir);

        MainCamTrm.parent = cameraParent;
        isMoveAfterPos = true;
        timer = 0;

        TransformCheckPointManagement.Instance.SetCheckpoint("AfterCheckpoint", PlayerTrm);

        _myRenderer.enabled = false;
    }

    private void ResetGravity()
    {
        if (normalVec == Vector3.zero || normalVec == Vector3.down) return;

        normalVec = Vector3.zero;

        TransformCheckPointManagement.Instance.SetCheckpoint("BeforeCheckpoint", PlayerTrm);
        GravityManager.Instance.ChangeGlobalGravityDirection(Vector3.down);

        MainCamTrm.parent = cameraParent;
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

            // 투 두 : 지금 대입이 아니라 - 연산을 하고 있기때문에 우앱이가 Set함수를 만들어주면 그거를 사용할거임
            //_movement.SetMouseY(-Mathf.Lerp(beforeCamRotY, 
            //                                afterCamRotY, 
            //                                timer));

            cameraParent.rotation = Quaternion.Lerp(beforeCamRot, afterCamRot, timer);

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
