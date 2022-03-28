using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

using Player.Status;
using Commands.Movement;
using Player.Utils;
using Objects.Utils;
using Gravity.Object.Management;

namespace Player.Movement
{
   /// <summary>
   /// 기본적인 움직임을 구현한 클레스
   /// </summary>
   [RequireComponent(typeof(Rigidbody))]
   public class PlayerMovement : MonoBehaviour,
                                 IMoveable,
                                 IJumpable,
                                 ICrouchable
   {

      private Rigidbody rigid;
      private GroundChecker groundChecker;

      public System.Action OnJump;
      public System.Action OnLanded;

      private void Awake()
      {
         rigid = GetComponent<Rigidbody>();
         groundChecker = GetComponentInChildren<GroundChecker>();
         OnJump += () => { };
         OnLanded += () => { };
      }

      #region Movement

      public void MoveFoward()
      {
         if (!PlayerStatus.Moveable) return;
         PlayerMoveDelta.Instance.AddYDelta(PlayerValues.speed);
      }

      public void MoveBackword()
      {
         if (!PlayerStatus.Moveable) return;
         PlayerMoveDelta.Instance.AddYDelta(-PlayerValues.speed);
      }

      public void MoveLeft()
      {
         if (!PlayerStatus.Moveable) return;
         PlayerMoveDelta.Instance.AddXDelta(-PlayerValues.speed);
      }

      public void MoveRight()
      {
         if (!PlayerStatus.Moveable) return;
         PlayerMoveDelta.Instance.AddXDelta(PlayerValues.speed);
      }

      public void DashStart()
      {
         if (PlayerStatus.IsCrouching)
         {
            PlayerUtils.Instance.SetStanded();
         }

         PlayerUtils.Instance.SetRunning();
      }

      public void DashStop()
      {
         if (PlayerStatus.IsCrouching) return;

         PlayerUtils.Instance.SetWalking();
      }

      #endregion // Movement



      #region Jump

      public void Jump()
      {
         if (!PlayerStatus.Jumpable) return;

         if (PlayerStatus.IsCrouching)
         {
            Logger.Log("웅크린 상태 중 Jump 명령.");
            PlayerUtils.Instance.SetStanded();
            return;
         }

         Vector3 force = new Vector3(1.0f, Mathf.Sqrt(2.0f * 9.8f * PlayerValues.JumpHeight), 1.0f); // TODO: Cache?
         Vector3 gravityDir = GravityManager.Instance.GetGlobalGravityDirection();

         force.x *= gravityDir.x;
         force.y *= gravityDir.y;
         force.z *= gravityDir.z;

         rigid.velocity = -force;

         PlayerStatus.CameraShakeCorrection = false;
         PlayerStatus.HeadBob = false;
         PlayerStatus.IsJumping = true;

         OnJump();
      }

      #endregion // Jump

      public void Crouch()
      {
         if (!PlayerStatus.IsCrouching)
         {
            PlayerUtils.Instance.SetCrouched();
         }
         else
         {
            PlayerUtils.Instance.SetStanded();
         }
      }

      private void Update()
      {
         Vector3 delta = PlayerMoveDelta.Instance.GetDelta();
         delta.z = delta.y;
         delta.y = 0.0f;
         transform.Translate(delta * Time.deltaTime);

         if (delta == Vector3.zero)
            PlayerStatus.IsMoving = false;
      }

      public void OnGround() { }
   }
}
