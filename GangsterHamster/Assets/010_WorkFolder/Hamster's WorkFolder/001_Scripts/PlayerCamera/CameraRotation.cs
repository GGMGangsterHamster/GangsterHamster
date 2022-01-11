using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject cameraObj;
    public Transform playerTrm;

    public float rotationSpeed = 3f;

    float mouseY;
    float mouseX;

    private void Awake()
    {
        mouseY = 0;
        mouseX = 0;
    }

    private void Update()
    {
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -80, 80);

        cameraObj.transform.rotation = Quaternion.Euler(new Vector3(
            mouseY,
            mouseX,
            0));

        cameraObj.transform.position = playerTrm.position;

        playerTrm.rotation = Quaternion.Euler(new Vector3(
            0,
            cameraObj.transform.rotation.eulerAngles.y,
            0));
    }
}
