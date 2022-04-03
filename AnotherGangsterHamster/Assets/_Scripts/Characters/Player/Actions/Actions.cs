using Physics.Gravity;
using UnityEngine;

namespace Characters.Player.Actions
{
   [RequireComponent(typeof(Rigidbody))]
   public class Actions : MonoBehaviour, IActionable
   {
      private Rigidbody _rigid;

      Vector3 _jumpForce;

      private void Awake() {
         _jumpForce =
                  new Vector3(1.0f,
                           Mathf.Sqrt(2.0f *
                                      9.8f * // Gravity
                                      PlayerValues.JumpHeight),
                           1.0f);
         _rigid = GetComponent<Rigidbody>();
      }

      public void CrouchEnd()
      {
         throw new System.NotImplementedException();
      }

      public void CrouchStart()
      {
         throw new System.NotImplementedException();
      }

      public void DashStart()
      {
         if(!PlayerStatus.OnGround) return;
         if(PlayerStatus.IsCrouching)
         {

         }
         
      }

      public void DashEnd()
      {
      
      }


      public void Jump()
      {
         if(!PlayerStatus.Jumpable) return;
         if(PlayerStatus.IsCrouching)
         {
            return;
         }

         Vector3 force = _jumpForce;
         Vector3 gravityDir = GravityManager.GetGlobalGravityDirection();

         force.x *= gravityDir.x;
         force.y *= gravityDir.y;
         force.z *= gravityDir.z;

         _rigid.velocity = -force;

         PlayerStatus.IsJumping = true;
      }
   }
}