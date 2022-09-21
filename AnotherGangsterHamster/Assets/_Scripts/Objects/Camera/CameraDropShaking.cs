using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;
using Characters.Player.Actions;
using Matters.Gravity;
using UnityEngine.Events;
using System;

public class CameraDropShaking : MonoBehaviour
{
    [Header("셰이킹 변수")]
    [SerializeField] private float depth;
    [SerializeField] private float downDuration;
    [SerializeField] private float upDuraion;

    [Header("낙하 조건 변수")]
    [SerializeField] private float dropVelocity = 9f;
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rigid;

    private bool canShake = false;
    private float defaultHeight;
    private const float HALF_PI = Mathf.PI / 2;

    public bool isOnlyDropCamera { get; private set; }

    void Start()
    {
        defaultHeight = transform.localPosition.y;
    }

    void Update()
    {
        if (!PlayerStatus.OnGround)
        {
            if (GravityManager.GetGlobalGravityDirection() == Vector3.down)
            {
                if (rigid.velocity.y <= -dropVelocity)
                {
                    canShake = true;
                }
            }
            else if (GravityManager.GetGlobalGravityDirection() == Vector3.up)
            {
                if (rigid.velocity.y >= dropVelocity)
                {
                    canShake = true;
                }
            }
            else if (GravityManager.GetGlobalGravityDirection() == Vector3.left)
            {
                if (rigid.velocity.x <= -dropVelocity)
                {
                    canShake = true;
                }
            }
            else if (GravityManager.GetGlobalGravityDirection() == Vector3.right)
            {
                if (rigid.velocity.x >= dropVelocity)
                {
                    canShake = true;
                }
            }
            else if (GravityManager.GetGlobalGravityDirection() == Vector3.back)
            {
                if (rigid.velocity.z <= -dropVelocity)
                {
                    canShake = true;
                }
            }
            else if (GravityManager.GetGlobalGravityDirection() == Vector3.forward)
            {
                if (rigid.velocity.z >= dropVelocity)
                {
                    canShake = true;
                }
            }
        }
    }

    public void DropShaking() // 충돌 시 이벤트
    {
        if (canShake)
        {
            isOnlyDropCamera = true;
            StartCoroutine(DropShake());          
            canShake = false;
        }
    }

    IEnumerator DropShake()
    {
        float downVelocity = HALF_PI / downDuration;  // 하강 속력
        float t = Mathf.PI;                           // 마이너스로 가기 위한 0 세팅
         
        while (t < Mathf.PI * 1.5f) // -1 까지만
        {
            t += Time.deltaTime * downVelocity;
            float y = defaultHeight + Mathf.Sin(t) * depth; // 기존 높이 - Sin * 증폭값
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            yield return null;
        }

        float downHeight = transform.localPosition.y;
        float upVelocity = HALF_PI / upDuraion;       // 상승 속력

        t = 0f;

        while (t < Mathf.PI * 0.5f) // 0 으로
        {
            t += Time.deltaTime * upVelocity;
            float y = downHeight + Mathf.Sin(t) * depth;
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            yield return null;
        }

        isOnlyDropCamera = false;
    }
}