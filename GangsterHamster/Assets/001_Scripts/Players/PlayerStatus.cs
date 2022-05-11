using UnityEngine;

namespace Player.Status
{
   static public class PlayerStatus
   {
      // 상태 변수
      static public bool IsRunning { get; set; } = false;
      static public bool IsMoving { get; set; } = false;
      static public bool IsJumping { get; set; } = false;
      static public bool IsCrouching { get; set; } = false;
      static public bool OnGround { get; set; } = true;

      // 플레그 변수
      static public bool Damaged { get; set; } = false;

      // 행동 가능 상태 변수
      static public bool Moveable { get; set; } = true;
      static public bool Jumpable { get; set; } = true;
      static public bool Crouchable { get; set; } = true;
      static public bool Runable { get; set; } = true;
      static public bool CameraShakeCorrection { get; set; } = true;
      static public bool HeadBob { get; set; } = true;
   }

}