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
    // 일단은 BoxCollider를 기준으로 한다 만약 Sphere와 같이 각도를 구하기 애매한 것은 무기가 다시 돌아오게 한다.

    [SerializeField]
    private float shotSpeed = 5;
    [SerializeField]
    private Transform CamPosTrm;

    private WeaponManagement wm;
    private Rigidbody _myRigid;
    private Collider _myCol;
    private Transform playerTrm; // 플레이어의 Trm
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

            ChangeGravity();
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
            isChangedGravity = true;
            // 부딪힌 그 오브젝트의 면에서 수직 방향으로 중력을 바꾼다 
            // 만약 이미 바꿔져 있는 상태라면 그냥 아무것도 안하고 넘긴다.
            normalVec = collision.contacts[0].normal;
            float angle = Vector3.Angle(playerTrm.up, collision.contacts[0].normal);

            GravityManager.Instance.ChangeGlobalGravityDirection(-normalVec);
            playerTrm.rotation = Quaternion.Euler(new Vector3(normalVec.z != 0 ? normalVec.z > 0 ? angle : -angle : normalVec.y != 0 ? normalVec.y > 0 ? 0 : angle * 2 : 0
                                                            , 0
                                                            , normalVec.x != 0 ? normalVec.z > 0 ? -angle : angle : 0
            ));
            // 여기서 무기를 멈추게 해야 함
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
