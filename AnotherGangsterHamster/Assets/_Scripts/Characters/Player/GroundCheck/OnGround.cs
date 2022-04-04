using UnityEngine;

namespace Characters.Player.GroundCheck
{
   public class OnGround : MonoBehaviour, IGroundCallback
   {
      public void ExitGround()
      {
         PlayerStatus.OnGround   = false;
         PlayerStatus.IsJumping  = true;
         PlayerStatus.Jumpable   = false;
      }

      void IGroundCallback.OnGround()
      {
         PlayerStatus.IsJumping  = false;
         PlayerStatus.OnGround   = true;
         PlayerStatus.Jumpable   = true;
      }
   }
}