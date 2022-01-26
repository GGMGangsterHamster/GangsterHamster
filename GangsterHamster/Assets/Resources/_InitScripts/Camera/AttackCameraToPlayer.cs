using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Camera;
using OpenRC;

namespace Objects.Init
{

    public class AttackCameraToPlayer : MonoBehaviour, IInitBase
    {
        private Transform playerTrm = null;

        public void Call(object param)
        {
            
        }

        public void Depend(MonoBehaviour mono)
        {
            playerTrm = GameObject.FindGameObjectWithTag("PLAYER_CAMERA_LOCK").transform;
        }

        // public void Start(object param)
        public void Start()
        {
            if(
                true
                #if UNITY_EDITOR
                // param.ToString().CompareTo("HanTestScene") == 0 || 
                #endif
                // param.ToString().CompareTo("Stage") == 0
                )
            {
                
            }
            playerTrm = GameObject.FindGameObjectWithTag("PLAYER_CAMERA_LOCK").transform;
            Debug.Log(playerTrm ==null);
            AttachCameraToObject.Instance.SetAttachPosition(playerTrm);
        }

        public void Stop()
        {
            
        }
    }
}