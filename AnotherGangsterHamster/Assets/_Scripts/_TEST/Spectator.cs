using Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 코드를 보는 사람에게 알립니다.
// 이건 임시에요 코드 보고 뭐라하지 마세요
// - 햄 -
public class Spectator : MonoSingleton<Spectator>
{
    private bool isSpectorMode;
    private Transform mainCam;
    private float rotY = 0.0f;

    public void StartSpectorMode()
    {
        Utils.UnlockCursor();
        isSpectorMode = true;

        Camera.main.transform.parent = transform;
        Camera.main.transform.position = Vector3.zero;

        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        if(isSpectorMode)
        {
            transform.position += mainCam.forward * Input.GetAxis("Vertical") * Time.deltaTime * 5;
            transform.position += mainCam.right * Input.GetAxis("Horizontal") * Time.deltaTime * 5;


            transform.rotation =
                     transform.rotation
                     * Quaternion.Euler(0.0f,
                                        Input.GetAxis("Mouse X") * 5,
                                        0.0f);


            rotY += -Input.GetAxis("Mouse Y") * PlayerValues.MouseSpeed;
            rotY = Mathf.Clamp(rotY, -89f, 89f);

            mainCam.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
        }
    }
}
