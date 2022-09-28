using Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �ڵ带 ���� ������� �˸��ϴ�.
// �̰� �ӽÿ��� �ڵ� ���� �������� ������
// - �� -
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
