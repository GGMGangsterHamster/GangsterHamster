namespace Player
{
    public class PlayerValues : Singleton<PlayerValues>, ISingletonObject
    {
        // public const float WalkingSpeed = 1.3f;
        // public const float DashSpeed = 2.0f;
        // public const float CrouchSpeed = 0.8f;
        public const float WalkingSpeed = 2.2f;
        public const float DashSpeed = 5.0f;
        public const float CrouchSpeed = 1.2f;

        public const float PlayerHeight = 1.8f;
        public const float PlayerCrouchHeight = 1.0f;

        public const float JumpForce = 5.3f;


        #region 미리 탐구한 플레이어 변수들

        public const float PlayerYPos = 0.9f;
        public const float PlayerYScale = 1.8f;

        public const float PlayerCrouchYPos = 0.5f;
        public const float PlayerCrouchYScale = 1.0f;

        #endregion

        public float speed = WalkingSpeed;
        public float mouseSpeed = 2.0f;
    }
}