using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Weapons.Checkpoint
{
    public class CheckpointManager : MonoBehaviour
    {
        // 3번 무기에만 사용할거라 굳이 싱글톤으로 만들지는 안ㅇ므

        [HideInInspector] public Transform startCheckpoint;
        [HideInInspector] public Transform endCheckpoint;



        private void Awake()
        {
            startCheckpoint = new GameObject("StartCheckpoint").transform;
            startCheckpoint.parent = transform;

            endCheckpoint = new GameObject("EndCheckpoint").transform;
            endCheckpoint.parent = transform;
        }

        public void SetStartCheckpoint(Vector3 dir)
        {
            startCheckpoint.rotation = Quaternion.LookRotation(dir);
        }

        public void SetEndCheckpoint(Vector3 normalVec)
        {
            endCheckpoint.rotation = Quaternion.LookRotation(-normalVec);

            endCheckpoint.rotation *= Quaternion.LookRotation(Vector3.up);
        }
    }
}