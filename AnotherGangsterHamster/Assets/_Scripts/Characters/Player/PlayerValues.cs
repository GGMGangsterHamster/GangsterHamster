namespace Characters.Player
{
   /// <summary>
   /// 플레이어의 수치가 적혀 있는 구조체
   /// </summary>
   static public class PlayerValues
   {
      public const float WalkingSpeed = 2.2f;
      public const float DashSpeed = 5.0f;
      public const float CrouchSpeed = 1.2f;
      
      public const float JumpHeight = 1.1f;

      public const float InteractionMaxDistance = 1.0f;

      static private float _speed = WalkingSpeed;
      static public float Speed
      {
         get => _speed;
         set => _speed = value;
      }

      static private float _mouseSpeed = 2.0f;
      static public float MouseSpeed
      {
         get => _mouseSpeed;
         set => _mouseSpeed = value;
      }
   }
}