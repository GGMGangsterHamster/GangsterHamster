using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStabilization : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float magnitude;
    private Camera mainCam;

    void Start()
    {
        mainCam = FindObjectOfType<Camera>();
        cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, Mathf.Clamp(magnitude, 0, magnitude));   
    }

    void Update()
    {
        transform.LookAt(cameraTarget);
    }
}
