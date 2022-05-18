using UnityEngine;

namespace Matters.Gravity
{
   /// <summary>
   /// 중력 영향을 받는 모든 오브젝트가 가지고 있어야 하는 컴포넌트
   /// </summary>
   [RequireComponent(typeof(Rigidbody))]
   public class GravityAffectedObject : MonoBehaviour
   {
      public bool Enabled { get; set; } = true;

      private bool _affectedByGlobalGravity = true;
      public bool AffectedByGlobalGravity
      {
         get => _affectedByGlobalGravity;
         set
         {
            _affectedByGlobalGravity = value;
            // if (value) // 전역 중력이 활성화되면 기본값으로 초기화 함
            // {
            //    _affectedByGlobalGravity      = value;
            //    _individualGravity._direction  = Vector3.down;
            //    _individualGravity._force      = 9.8f;
            // }
            // else
            //    _affectedByGlobalGravity = value;
         }
      }

      private GravityValue _individualGravity = null; // 지역 중력
      private Rigidbody _rigid;

      private void Awake()
      {
         _rigid = GetComponent<Rigidbody>();
         _individualGravity = new GravityValue(Vector3.down);
      }

      public void SetIndividualGravity(Vector3 direction, float force = 9.8f)
      {
         _individualGravity._direction = direction;
         _individualGravity._force = force;
      }

      /// <summary>
      /// 중력을 가합니다.
      /// </summary>
      /// <param name="gravity">중력 인스턴스</param>
      public virtual void Gravity(GravityValue globalGravity)
      {
         if (!Enabled) return;

         if (AffectedByGlobalGravity) // 전역 중력 영향 받는 오브젝트 인 경우
            _rigid.AddForce(globalGravity._direction.normalized * globalGravity._force,
                           ForceMode.Acceleration);
         else // 전역 중력 영향 안 받는 오브젝트 인 경우
            _rigid.AddForce(_individualGravity._direction.normalized * _individualGravity._force,
                           ForceMode.Acceleration);
      }
   }
}