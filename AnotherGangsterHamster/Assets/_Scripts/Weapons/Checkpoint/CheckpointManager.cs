using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Weapons.Checkpoint
{
    public class CheckpointManager : MonoBehaviour
    {
        // 3�� ���⿡�� ����ҰŶ� ���� �̱������� �������� �Ȥ���

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
            //startCheckpoint.rotation = Quaternion.LookRotation(-startCheckpoint.up);
        }

        public void SetEndCheckpoint(Vector3 dir)
        {
            //endCheckpoint.rotation = Quaternion.LookRotation(dir);
            //endCheckpoint.rotation = Quaternion.LookRotation(-endCheckpoint.up);

            //endCheckpoint.rotation = Quaternion.Euler(new Vector3(dir.x * 90, 90 + -(dir.z * 90), 0)) * endCheckpoint.rotation;
        }
    }
}