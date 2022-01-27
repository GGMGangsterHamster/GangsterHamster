using System.Collections;
using System.Collections.Generic;
using Obejcts.Utils;
using UnityEngine;

namespace Objects.Utils
{
    public class GroundChecker : MonoBehaviour
    {
        const string GROUND = "GROUND";
        IGroundCallbackObject callback;

        private void Awake()
        {
            callback = GetComponentInChildren<IGroundCallbackObject>();
            if(callback == null) {
                Log.Debug.Log($"GroundChecker > {gameObject.name} 의 자식 오브젝트에서 IGroundCallbackObejct 을 찾을 수 없습니다.", Log.LogLevel.Fatal);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag(GROUND)) {
                callback.OnGround();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag(GROUND)) {
                callback.ExitGround();
            }
        }

        // TODO: 아레쪽으로 Ray
    }
}