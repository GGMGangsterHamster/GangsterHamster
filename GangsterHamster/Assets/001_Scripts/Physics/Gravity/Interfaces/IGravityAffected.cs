using UnityEngine;

namespace Gravity
{
    // 중력 영향을 받는 오브젝트가 구체화해야 함
    public interface IGravityAffected
    {
        /// <summary>
        /// 중력 영향 적용 스크립트
        /// </summary>
        /// <param name="dir">중력 작용 방향 (Normalized)</param>
        /// <param name="amount">중력의 세기</param>
        public void Gravity(Vector3 dir, float amount);
    }
}