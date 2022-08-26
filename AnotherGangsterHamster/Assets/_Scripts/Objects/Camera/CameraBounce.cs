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
        if (!PlayerStatus.OnGround) // ����
        {
            if (minPlayerPosY < player.localPosition.y) // �ְ��� �Ǻ�
            {
                maxHeight = player.localPosition.y;
            }
            else
            {
                maxHeight = minPlayerPosY;
            }

            hasAir = true;
        }

        if (PlayerStatus.OnGround) // ��
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
        float v0 = -gravity; // y ������ �ʱ� �ӵ�

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / bounceTime;

            float y = minCamPosY + (v0 * percent) + (gravity * percent * percent);

            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            
            yield return null;
        }

        // ����ġ
        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
    }
}
