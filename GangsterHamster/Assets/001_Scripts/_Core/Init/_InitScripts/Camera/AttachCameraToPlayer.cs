using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Camera;
using OpenRC;

namespace Objects.Init
{

    public class AttachCameraToPlayer : InitBase
    {
        private Transform playerTrm = null;

        public override string Name => "Attach Camera To Player";
        public override RunLevel RunLevel => RunLevel.OnSceneLoad;

        public override void Call(object param)
        {
            if(
                true
                #if UNITY_EDITOR
                // param.ToString().CompareTo("HanTestScene") == 0 ||
                #endif
                // param.ToString().CompareTo("Stage") == 0
                )
            {
                AttachCameraToObject.Instance.SetAttachPosition(playerTrm);
            }
          
        }

        public override void Depend(MonoBehaviour mono)
        {
            playerTrm = GameObject.FindGameObjectWithTag("PLAYER_CAMERA_LOCK").transform;
        }

        public override void Stop() { }
    }
}