using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parentObj�� ��ġ�� �����ϰ� �ϴ� Ŭ����
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
