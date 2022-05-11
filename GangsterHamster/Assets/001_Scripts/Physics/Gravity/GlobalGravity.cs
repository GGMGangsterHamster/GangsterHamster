using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// 전역 중력 클레스
    /// </summary>
    class GlobalGravity
    {
        /// <summary>
        /// 중력 작용 방향
        /// </summary>
        public Vector3 direction;

        /// <summary>
        /// 중력 크기
        /// </summary>
        public float force;

        /// <summary>
        /// 전역 중력 클레스를 생성합니다.
        /// </summary>
        /// <param name="direction">중력 작용 방향</param>
        /// <param name="force">중력의 크기</param>
        public GlobalGravity(Vector3 direction, float force = 9.8f)
        {
            this.direction = direction;
            this.force = force;
        }
    }
}