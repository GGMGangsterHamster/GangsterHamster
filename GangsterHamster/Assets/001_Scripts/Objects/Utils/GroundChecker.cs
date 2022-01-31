using System.Collections;
using System.Collections.Generic;
using Obejcts.Utils;
using UnityEngine;

namespace Objects.Utils
{
    public class GroundChecker : Singleton<GroundChecker>, ISingletonObject
    {
        const string GROUND = "GROUND";
        // private IGroundCallbackObject _callback;
        // [SerializeField] private float _groundDistance = 0.05f;

        private int _targetLayer;

        public GroundChecker()
        {
            // _callback = GetComponentInChildren<IGroundCallbackObject>();
            // if(_callback == null) {
            //     Log.Debug.Log($"GroundChecker > {gameObject.name} 의 자식 오브젝트에서 IGroundCallbackObejct 을 찾을 수 없습니다.", Log.LogLevel.Fatal);
            // }

            _targetLayer = LayerMask.GetMask(GROUND);
            Debug.Log(_targetLayer);
        }

        // private void Update()
        // {
        //     RaycastHit hit;
        //     if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, _groundDistance, _targetLayer)) {
        //         _callback.OnGround();
        //     }
        //     else {
        //         _callback.ExitGround();
        //         Debug.LogWarning("A");
        //     }
        // }

        public bool CheckGround(Transform transform, float groundDistance)
        {
            Vector3 pos = transform.position;
            pos.y += 0.05f;
            bool res = Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out RaycastHit hit, groundDistance, _targetLayer);
            return res;
        }

        // private void OnTriggerStay(Collider other)
        // {
        //     if(other.gameObject.CompareTag(GROUND)) {
        //         _callback.OnGround();
        //     }
        // }

        // private void OnTriggerExit(Collider other)
        // {
        //     if(other.gameObject.CompareTag(GROUND)) {
        //         _callback.ExitGround();
        //     }
        // }

        // TODO: 아레쪽으로 Ray
    }
}