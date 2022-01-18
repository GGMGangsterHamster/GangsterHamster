using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Objects.Movement.Camera;
using Objects.Bezier;

public class BezierTest : MonoBehaviour
{
    public BezierObject bezier;

    private void Start()
    {
        CameraMovementManager.Instance.BezierMove(2.0f, bezier, this, true, null, () => {
            Debug.Log("Arrived");
        }, () => {
            Debug.Log("Returned");
            CameraMovementManager.Instance.LinearMove(2.0f, new Vector3(0.0f, 0.0f, -10.0f), new Vector3(0.0f, 2.0f, -10.0f), this, null, () => {
                Debug.Log("Arrived");
            });
        });
    }
}

