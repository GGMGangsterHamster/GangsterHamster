using Objects;
using UnityEngine;

namespace Characters.Player.OnGround
{
   public class OnGround : MonoBehaviour
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

         Collider.sharedMaterial.staticFriction = 0.0f;
         Collider.sharedMaterial.dynamicFriction = 0.0f;
      }

      public void StayGround()
      {
         PlayerStatus.IsJumping = false;
         PlayerStatus.OnGround = true;
         PlayerStatus.Jumpable = true;

         Collider.sharedMaterial.staticFriction = 1.0f;
         Collider.sharedMaterial.dynamicFriction = 1.0f;
      }

      public void AtGround()
      {
         PlayerStatus.IsJumping = false;
         PlayerStatus.OnGround = true;
         PlayerStatus.Jumpable = true;

         Collider.sharedMaterial.staticFriction = 1.0f;
         Collider.sharedMaterial.dynamicFriction = 1.0f;
      }
   }
}