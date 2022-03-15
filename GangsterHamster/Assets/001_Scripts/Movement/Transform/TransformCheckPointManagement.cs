using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCheckPointManagement : MonoSingleton<TransformCheckPointManagement>
{
    private Dictionary<string, Transform> checkpointsDic = new Dictionary<string, Transform>();

    [SerializeField] private GameObject checkpointPrefab;

    // 체크포인트 추가하는 함수
    private void AddCheckpoint(string key)
    {
        Transform obj = Instantiate(checkpointPrefab).transform;
        checkpointsDic.Add(key, obj);
    }

    // 체크포인트 수정하는 함수
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

    // 체크포인트 리턴하는 함수
    public Transform GetCheckPoint(string key)
    {
        return checkpointsDic[key];
    }
}
