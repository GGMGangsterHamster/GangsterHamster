using Objects;
using UnityEngine;

namespace Characters.Player.GroundCheck
{
   public class OnGround : MonoBehaviour, ICollisionEventable
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

      public void Active(GameObject other)
      {
         PlayerStatus.IsJumping = false;
         PlayerStatus.OnGround = true;
         PlayerStatus.Jumpable = true;

         Collider.material.frictionCombine = PhysicMaterialCombine.Average;
      }

      public void Deactive(GameObject other)
      {
         PlayerStatus.OnGround = false;
         PlayerStatus.IsJumping = true;
         PlayerStatus.Jumpable = false;

         Collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
      }
   }
}