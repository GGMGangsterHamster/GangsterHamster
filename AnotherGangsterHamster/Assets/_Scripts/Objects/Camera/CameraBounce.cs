using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;

public class CameraBounce : MonoBehaviour
{
    public Transform player;
    public float bounceTime = 1f;

    private RaycastHit hit;
    private float minPlayerPosY;
    private float minCamPosY = 0.5f;
    private float maxHeight = 0f;
    private float gravity = -1f;
    private bool hasAir = false;
    private bool canBounce = false;

    void Update()
    {
        Debug.Log(player.localPosition.y);
        if (!PlayerStatus.OnGround) // 공중
        {
            if (minPlayerPosY < player.localPosition.y) // 최고점 판별
            {
                maxHeight = player.localPosition.y;
            }
            else
            {
                maxHeight = minPlayerPosY;
            }

            hasAir = true;
        }

        if (PlayerStatus.OnGround) // 땅
        {
            minPlayerPosY = player.localPosition.y;

            if (hasAir)
            {
                if (Mathf.Abs(player.localPosition.y - maxHeight) >= 4)
                {
                    hasAir = false;
                    StartCoroutine(Bouncing());
                }
            }
        }
    }

    void BounceChecker()
    {
        Physics.Raycast(player.position, Vector3.down, out hit, Mathf.Infinity);
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

        // 보정치
        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
    }
}
