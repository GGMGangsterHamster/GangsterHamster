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
    }
}
