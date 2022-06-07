using Objects;
using UnityEngine;

namespace Characters.Player.GroundCheck
{
   public class OnGround : MonoBehaviour, IGroundCallback
   {
      private CapsuleCollider _collider = null;
      private CapsuleCollider Collider
      {
         get
         {
            if (_collider == null)
            {
               _collider = GameObject.FindWithTag("PLAYER_BASE")
                                     .GetComponent<CapsuleCollider>();
            }

            return _collider;
         }
      }

      public void ExitGround()
      {
         PlayerStatus.OnGround = false;
         PlayerStatus.IsJumping = true;
         PlayerStatus.Jumpable = false;
         Utils.ExecuteCallback(this.transform, false);
      }

      void IGroundCallback.OnGround()
      {
         PlayerStatus.IsJumping = false;
         PlayerStatus.OnGround = true;
         PlayerStatus.Jumpable = true;
         Utils.ExecuteCallback(this.transform, true);
      }
   }
}