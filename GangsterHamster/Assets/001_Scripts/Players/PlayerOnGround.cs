using System.Collections;
using System.Collections.Generic;
using Obejcts.Utils;
using Player.Status;
using UnityEngine;

namespace Player.Movement
{

    public class PlayerOnGround : MonoBehaviour, IGroundCallbackObject
    {
        public void ExitGround()
        {
            Debug.Log("땅바이바이");
            PlayerStatus.Instance.OnGround = false;
        }

        public void OnGround()
        {
            Debug.Log("땅하이하이");
            PlayerStatus.Instance.OnGround = true;
        }
    }

}