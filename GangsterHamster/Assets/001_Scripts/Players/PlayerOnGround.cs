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
         PlayerStatus.OnGround = false;
         PlayerStatus.HeadBob = false;
      }

      public void OnGround()
      {
         PlayerStatus.IsJumping = false;
         PlayerStatus.OnGround = true;
         PlayerStatus.Jumpable = true;
         PlayerStatus.CameraShakeCorrection = true;
         PlayerStatus.HeadBob = true;
      }
   }

}