using Physics.Gravity;
using UnityEngine;

namespace Characters.Player.Actions
{
   [RequireComponent(typeof(Rigidbody))]
   public class Actions : MonoBehaviour, IActionable
   {
      private Rigidbody _rigid;

      private Transform _playerTrm = null;
      private Transform PlayerTrm 
      {
         get
         {
            if (_playerTrm == null)
            {
               _playerTrm = GameObject.FindWithTag("PLAYER").transform;
            }

            return _playerTrm;
         }
      }

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

      public void CrouchStart()
      {
         Vector3 targetScale  = PlayerTrm.localScale;
         Vector3 targetPos    = PlayerTrm.localPosition;

         PlayerStatus.IsCrouching   = true;
         PlayerValues.Speed         = PlayerValues.CrouchSpeed;

         targetScale.y  = PlayerValues.PlayerCrouchYScale;
         targetPos.y    = PlayerValues.PlayerCrouchYPos;

         Collider.height         = PlayerValues.PlayerCrouchHeight;
         Collider.center         = new Vector3(0.0f, Collider.height / 2.0f, 0.0f);
         PlayerTrm.localScale    = targetScale;
         PlayerTrm.localPosition = targetPos;
      }

      public void CrouchEnd()
      {
         Vector3 targetScale  = PlayerTrm.localScale;
         Vector3 targetPos    = PlayerTrm.localPosition;

         PlayerStatus.IsCrouching   = false;
         PlayerValues.Speed         = PlayerValues.WalkingSpeed;

         targetScale.y  = PlayerValues.PlayerStandingYScale;
         targetPos.y    = PlayerValues.PlayerStandingYPos;

         Collider.height         = PlayerValues.PlayerStandingHeight;
         Collider.center         = new Vector3(0.0f, Collider.height / 2.0f, 0.0f);
         PlayerTrm.localScale    = targetScale;
         PlayerTrm.localPosition = targetPos;
      }


      public void DashStart()
      {
         if (!PlayerStatus.OnGround) return;

         if (PlayerStatus.IsCrouching)
            CrouchEnd();
         
         PlayerStatus.IsRunning = true;
         PlayerValues.Speed = PlayerValues.DashSpeed;
      }

      public void DashEnd()
      {
         if (PlayerStatus.IsCrouching) return;

         PlayerStatus.IsRunning = false;
         PlayerValues.Speed = PlayerValues.WalkingSpeed;
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