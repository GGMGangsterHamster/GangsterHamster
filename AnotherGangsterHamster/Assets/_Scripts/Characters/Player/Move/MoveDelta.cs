using UnityEngine;

namespace Characters.Player.Move
{
   /// <summary>
   /// 플레이어가 이동할 방향을 연산함
   /// </summary>
   public class MoveDelta : MonoBehaviour
   {
      private Vector3 _delta;    // 플레이어 바라보는 방향에 따라 연산됨
      private Vector3 _rawDelta; // 플레이어 바라보는 방향과 상관 없이 World 로 연산

      private void Awake()
      {
         _delta    = Vector2.zero;
         _rawDelta = Vector3.zero;
      }

      public void SetRawDelta(Vector3 rawDelta) => _rawDelta  = rawDelta;
      public void AddRawDelta(Vector3 rawDelta) => _rawDelta += rawDelta;

      public void SetDelta(Vector3 delta) => _delta  = delta;
      public void AddDelta(Vector3 delta) => _delta += delta;

      public void AddXDelta(float x) => _delta.x += x;
      public void AddYDelta(float y) => _delta.y += y;
      public void AddZDelta(float z) => _delta.z += z;

      /// <summary>
      /// target 의 회전에 따른 delta 를 계산한 뒤 Raw delta 를 더해 반환합니다.
      /// </summary>
      /// <param name="target">회전 계산 기준 Transform</param>
      /// <returns>계산된 Delta</returns>
      public Vector3 Calculate(Transform target, bool keepDelta = false)
      {
         float scalar = _delta.magnitude;
         Vector3 finalDelta =
               (target.TransformDirection(_delta).normalized * scalar + _rawDelta)
               * Time.deltaTime;

         if (!keepDelta)
         {
            _rawDelta = Vector3.zero;
            _delta = Vector3.zero;
         }


         return finalDelta;
      }


   }
}