using Characters.Player.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;

public class CameraHeadBob : MonoBehaviour
{
    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10f;
    
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Rigidbody player;

    private Camera mainCam;
    private float tpggleSpeed = 3f;
    private Vector3 startPos;

    void Awake()
    {
        startPos = mainCam.transform.localPosition;
    }

    void Start()
    {
        mainCam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!enable) return;

        CheckMotion();
        ResetPosition();
        mainCam.transform.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        mainCam.transform.localPosition += motion;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(player.velocity.x, 0, player.velocity.z).magnitude;

        if (speed < tpggleSpeed) return;
        if (!PlayerStatus.OnGround) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void ResetPosition()
    {
        if (mainCam.transform.localPosition == startPos) return;
        mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15f;
        return pos;
    }
}
