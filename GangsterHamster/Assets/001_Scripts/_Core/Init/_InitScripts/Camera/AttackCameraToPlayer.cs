using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Camera;
using OpenRC;

namespace Objects.Init
{

    public class AttackCameraToPlayer : IInitBase    
    {
        private Transform playerTrm = null;

        public void Depend(MonoBehaviour mono)
        {
            playerTrm = GameObject.FindGameObjectWithTag("PLAYER_CAMERA_LOCK").transform;
        }

        public void Start(object param)
        {
            if(
                #if UNITY_EDITOR
                param.ToString().CompareTo("HanTestScene") == 0 || 
                #endif
                param.ToString().CompareTo("Stage") == 0
                )
            {
                AttachCameraToObject.Instance.SetAttachPosition(playerTrm);
            }
        }

        public void Stop()
        {
            
        }
    }
}