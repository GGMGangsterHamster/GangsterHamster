namespace Characters.Player
{
   static public class PlayerStatus
   {
      // 상태 변수
      static public bool IsRunning     = false;
      static public bool IsMoving      = false;
      static public bool IsJumping     = false;
      static public bool IsCrouching   = false;
      static public bool OnGround      = true;

      // 플레그 변수
      static public bool Damaged = false;

      // 행동 가능 상태 변수
      static public bool Moveable   = true;
      static public bool Jumpable   = true;
      static public bool Crouchable = true;
      static public bool Runable    = true;
   }
}