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
            if(param.ToString().CompareTo("HanTestScene") == 0) {
                AttachCameraToObject.Instance.SetAttachPosition(playerTrm);
            }
        }

        public void Stop()
        {
            
        }
    }
}