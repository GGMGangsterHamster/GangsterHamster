namespace Characters.Player
{
   /// <summary>
   /// 플레이어의 수치가 적혀 있는 클레스
   /// </summary>
   static public class PlayerValues
   {

#region 이동 속도
      public const float WalkingSpeed  = 2.2f;
      public const float DashSpeed     = 4.0f;
      public const float CrouchSpeed   = 1.2f;
#endregion // 이동 속도
      

#region Transform::Y scale
      public const float JumpHeight = 1.1f;
      // 플레이어 베이스 안에 있는 네모난 정사각형 (임시)
      public const float PlayerStandingYScale   = 1.8f;
      public const float PlayerCrouchYScale     = 1.0f;

#endregion // Transform::Y scale


#region Transform::Y position
      // 플레이어 베이스 안에 있는 네모난 정사각형 (임시)
      public const float PlayerStandingYPos  = 0.9f;
      public const float PlayerCrouchYPos    = 0.5f;

#endregion // Transform::Y position


#region Player Height
      public const float PlayerStandingHeight = 1.8f;
      public const float PlayerCrouchHeight  = 1.0f;
#endregion // Player Height


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