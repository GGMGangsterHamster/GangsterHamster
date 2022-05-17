using UnityEngine;

namespace Characters.Player.Move
{
   /// <summary>
   /// 플레이어가 이동할 방향을 연산함
   /// </summary>
   [RequireComponent(typeof(Rigidbody))]
   public class MoveDelta : MonoBehaviour
   {
      [field: SerializeField]
      public bool CalculateByItself = false;

      private Rigidbody _rigid;

      private Vector3 _delta;    // 플레이어 바라보는 방향에 따라 연산됨
      private Vector3 _rawDelta; // 플레이어 바라보는 방향과 상관 없이 World 로 연산

      private void Awake()
      {
         _delta    = Vector2.zero;
         _rawDelta = Vector3.zero;
         _rigid = GetComponent<Rigidbody>();
      }

      public void SetRawDelta(Vector3 rawDelta) => _rawDelta  = rawDelta;
      public void AddRawDelta(Vector3 rawDelta) => _rawDelta += rawDelta;

      public void SetDelta(Vector3 delta) => _delta  = delta;
      public void AddDelta(Vector3 delta) => _delta += delta;

      public void AddXDelta() => ++_delta.x;
      public void AddYDelta() => ++_delta.y;
      public void AddZDelta() => ++_delta.z;


      public void SubXDelta() => --_delta.x;
      public void SubYDelta() => --_delta.y;
      public void SubZDelta() => --_delta.z;

      /// <summary>
      /// target 의 회전에 따른 delta 를 계산한 뒤 Raw delta 를 더해 반환합니다.
      /// </summary>
      /// <param name="target">회전 계산 기준 Transform</param>
      /// <param name="deltaMultiply">delta * this</param>
      /// <param name="globalDelta">delta based on word coord</param>
      /// <param name="dontMultDeltatimeToRaw">raw delta * Time.deltatime</param>
      /// <returns>계산된 Delta</returns>
      public Vector3 Calculate(Transform target,
                               float deltaMultiply = 1.0f,
                               bool globalDelta = false,
                               bool dontMultDeltatimeToRaw = false,
                               bool keepDelta = false)
      {
         Vector3 delta = (globalDelta ? _delta : target.TransformDirection(_delta)).normalized
            * deltaMultiply * Time.deltaTime;

         Vector3 finalDelta = delta + _rawDelta * (dontMultDeltatimeToRaw ? 1 : Time.deltaTime);

         if (!keepDelta)
         {
            _rawDelta = Vector3.zero;
            _delta = Vector3.zero;
         }


         return finalDelta;
      }

      private void FixedUpdate()
      {
         if (CalculateByItself)
            _rigid.MovePosition(transform.position + Calculate(transform, 1.0f, false, true));
      }


   }
}