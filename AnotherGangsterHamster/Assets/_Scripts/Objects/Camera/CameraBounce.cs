using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;

public class CameraBounce : MonoBehaviour
{
    public Transform player;
    public Transform groundChecker;
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
        //if (!PlayerStatus.OnGround) // ����
        //{
        //    if (minPlayerPosY < player.position.y) // �ְ��� �Ǻ�
        //    {
        //        maxHeight = player.position.y;
        //    }
        //    else
        //    {
        //        maxHeight = minPlayerPosY;
        //    }

        //    hasAir = true;
        //}

        //if (PlayerStatus.OnGround) // ��
        //{
        //    minPlayerPosY = player.position.y;

        //    if (hasAir)
        //    {
        //        Debug.Log(Mathf.Abs(player.position.y - maxHeight));
        //        if (Mathf.Abs(player.position.y - maxHeight) >= 4)
        //        {
        //            hasAir = false;
        //            StartCoroutine(Bouncing());
        //        }
        //    }
        //}
    }

    // bool Is4M()
    // {
    //     bool is4m = Physics.Raycast(groundChecker.position, Vector3.down, 4f);
    // }

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

        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
    }
}
