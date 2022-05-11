using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parentObj의 위치만 공유하게 하는 클래스
/// </summary>
public class PositionParent : MonoBehaviour
{
    public GameObject parentObj;

    void Update()
    {
        transform.position = parentObj.transform.position;

        if (transform.childCount >= 1)
        {
            transform.GetChild(0).transform.localPosition = Vector3.zero;
        }
    }
}
