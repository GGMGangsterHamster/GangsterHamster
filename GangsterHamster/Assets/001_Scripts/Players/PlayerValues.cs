namespace Player
{
    static public class PlayerValues
    {
        #region 속도
        public const float WalkingSpeed = 2.2f;
        public const float DashSpeed = 5.0f;
        public const float CrouchSpeed = 1.2f;
        #endregion // 속도

        #region 높이 (길이)
        public const float PlayerHeight = 1.8f;
        public const float PlayerCrouchHeight = 1.0f;

        public const float JumpHeight = 0.7f;

        public const float PlayerYPos = 0.9f;
        public const float PlayerYScale = 1.8f;

        public const float PlayerCrouchYPos = 0.5f;
        public const float PlayerCrouchYScale = 1.0f;

        #endregion


        public const float InteractionMaxDistance = 1.0f;

        
        static public float speed = WalkingSpeed;
        static public float mouseSpeed = 2.0f;
    }
}