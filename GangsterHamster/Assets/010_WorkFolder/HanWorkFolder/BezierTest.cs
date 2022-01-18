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
        });
    }
}

