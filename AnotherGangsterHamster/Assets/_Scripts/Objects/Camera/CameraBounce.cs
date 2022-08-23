using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;

public class CameraBounce : MonoBehaviour
{
    public Transform player;
    public float magnitude;
    public float bounceHight;
    public float duration;
    private float bouncePos;
    private Vector3 defaultPos;

    private bool isAir;

    void Start()
    {
        defaultPos = transform.localPosition;
        bouncePos = defaultPos.y + bounceHight;
    }

    void Update()
    {
        Debug.Log("이즈 에어 " + isAir);

        if (player.position.y >= 4)
        {
            isAir = true;
        }

        if (!PlayerStatus.IsJumping && isAir)
        {
            Bounce();

            isAir = false;
        }
    }

    public void Bounce()
    {
        Debug.Log("비운스");
        StopCoroutine(Bouncing());
        StartCoroutine(Bouncing());
    }

    IEnumerator Bouncing()
    {
        while (transform.localPosition.y >= bouncePos)
        {
            transform.localPosition += new Vector3(0, magnitude * Time.deltaTime);
        }
        while (transform.position.y <= defaultPos.y)
        {
            transform.localPosition -= new Vector3(0, magnitude * Time.deltaTime);
        }

        yield return null;
    }
}
