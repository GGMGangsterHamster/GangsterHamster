using UnityEngine;

namespace Player.Status
{
    public class PlayerStatus : Singleton<PlayerStatus>, ISingletonObject
    {
        // 상태 변수
        public bool IsRunning { get; set; } = false;
        public bool IsMoving { get; set; } = false;
        public bool IsJumping { get; set; } = false;
        public bool IsCrouching { get; set; } = false;
        public bool OnGround { get; set; } = true;

        // 플레그 변수
        public bool Damaged { get; set; } = false;

        // 행동 가능 상태 변수
        public bool Moveable { get; set; } = true;
        public bool Jumpable { get; set; } = true;
        public bool Crouchable { get; set; } = true;
        public bool Runable { get; set; } = true;
    }

}