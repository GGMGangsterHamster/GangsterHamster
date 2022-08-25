using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;

public class CameraBounce : MonoBehaviour
{
    public Transform player;
    public float bounceTime = 1f;

    private float minPlayerPosY;
    private float minCamPosY;
    private float maxHeight = 0f;
    private float gravity = -1f;
    private bool hasAir = false;
    private bool canBounce;

    void Start()
    {
        minCamPosY = 0.5f;
    }

    void Update()
    {
        if (!PlayerStatus.OnGround) // 공중
        {
            if (minPlayerPosY < player.position.y) // 최고점 판별
            {
                maxHeight = player.position.y;
            }
            else
            {
                maxHeight = minPlayerPosY;
            }

            hasAir = true;
        }

        if (PlayerStatus.OnGround) // 땅
        {
            minPlayerPosY = player.position.y;

            if (hasAir)
            {
                if (Mathf.Abs(player.position.y - maxHeight) >= 4)
                {
                    hasAir = false;
                    StartCoroutine(Bouncing());
                }
            }
        }
    }

    IEnumerator Bouncing()
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity; // y 방향의 초기 속도

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / bounceTime;

            float y = minCamPosY + (v0 * percent) + (gravity * percent * percent);

            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
    }
}
