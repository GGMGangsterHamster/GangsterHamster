using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;
using Characters.Player.Move;

public class CameraHeadBobbing : MonoBehaviour
{
    public bool ForceBob { get; set; } = false;

    [SerializeField] private float frequency;  // ��鸮�� �󵵼�
    [SerializeField] private float magnitude;  // ��鸮�� ���� ����
    [SerializeField] private float rollbackSpeed;  // ���� Ʈ���������� ������ �ӵ�
    [SerializeField] private CameraDropShaking dropShaking;
    [SerializeField] private MoveDelta moveDelta;

    private float angleX = 0f;
    private float angleY = 270f;
    private Vector3 beginPos;

    void Start()
    {
        beginPos = transform.localPosition;
    }

    void Update()
    {
        if (ForceBob)
        {
            transform.localPosition = HeadBobbing();
            return;
        }

        if (!dropShaking.isOnlyDropCamera)
        {
            if (PlayerStatus.OnGround && PlayerStatus.IsMoving && !Utils.Compare(moveDelta.GetLastDelta(), Vector3.zero))
            {
                transform.localPosition = HeadBobbing();
            }
            else if (PlayerStatus.OnGround && PlayerStatus.IsMoving && Utils.Compare(moveDelta.GetLastDelta(), Vector3.zero))
            {
                ResetPosition();
            }
            if (!PlayerStatus.IsMoving || !PlayerStatus.OnGround)
            {
                ResetPosition();
            }
        }

    }

    Vector3 HeadBobbing() 
    {
        angleX += Time.deltaTime;
        angleY += Time.deltaTime;

        float x = Mathf.Cos(angleX * frequency) * magnitude * 1.5f;
        float y = Mathf.Sin(Mathf.PingPong(angleY * frequency, 180f) * 2) * magnitude + beginPos.y;

        return new Vector3(x, y, transform.localPosition.z);
    }

    void ResetPosition()
    {
        angleX = 0;
        angleY = 0;

        transform.localPosition = Vector3.Lerp(transform.localPosition, beginPos, Time.deltaTime * rollbackSpeed);
    }
}
