using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCheckPointManagement : MonoSingleton<TransformCheckPointManagement>
{
    private Dictionary<string, Transform> checkpointsDic = new Dictionary<string, Transform>();

    [SerializeField] private GameObject checkpointPrefab;

    // üũ����Ʈ �߰��ϴ� �Լ�
    private void AddCheckpoint(string key)
    {
        Transform obj = Instantiate(checkpointPrefab).transform;
        checkpointsDic.Add(key, obj);
    }

    // üũ����Ʈ �����ϴ� �Լ�
    public void SetCheckpoint(string key, Transform value)
    {
        if (!checkpointsDic.ContainsKey(key))
        {
            AddCheckpoint(key);
        }

        checkpointsDic[key].position = value.position;
        checkpointsDic[key].rotation = value.rotation;
        checkpointsDic[key].localScale = value.localScale;
    }

    // üũ����Ʈ �����ϴ� �Լ�
    public Transform GetCheckPoint(string key)
    {
        return checkpointsDic[key];
    }
}
