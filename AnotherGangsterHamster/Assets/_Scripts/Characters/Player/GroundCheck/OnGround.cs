using UnityEngine;

namespace Characters.Player.GroundCheck
{
   public class OnGround : MonoBehaviour, IGroundCallback
   {
      /// <summary>
      /// 바닥 떠날 시 호출됨
      /// </summary>
      public void ExitGround()
      {
         PlayerStatus.OnGround   = false;
         PlayerStatus.IsJumping  = true;
         PlayerStatus.Jumpable   = false;
      }

      /// <summary>
      /// 바닥이랑 붙어있으면 계속 호출됨 (고쳐아 함)
      /// </summary>
      void IGroundCallback.OnGround()
      {
         PlayerStatus.IsJumping  = false;
         PlayerStatus.OnGround   = true;
         PlayerStatus.Jumpable   = true;
      }
   }
}