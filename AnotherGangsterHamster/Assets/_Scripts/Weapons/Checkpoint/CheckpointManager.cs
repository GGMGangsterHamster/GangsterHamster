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

        public void SetEndCheckpoint(Vector3 dir)
        {
            endCheckpoint.rotation = Quaternion.LookRotation(dir);
            Debug.Log("endCheckpoint.down : " + -endCheckpoint.up);

            //endCheckpoint.rotation *= Quaternion.LookRotation(-endCheckpoint.up);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                endCheckpoint.rotation *= Quaternion.LookRotation(-endCheckpoint.up);
            }
        }
    }
}