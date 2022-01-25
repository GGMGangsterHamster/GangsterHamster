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
            PlayerStatus.Instance.OnGround = false;
        }

        public void OnGround()
        {
            PlayerStatus.Instance.OnGround = true;
        }
    }

}