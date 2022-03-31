using UnityEngine;

namespace Physics.Gravity
{
   /// <summary>
   /// 중력 영향을 받는 모든 오브젝트가 가지고 있어야 하는 컴포넌트
   /// </summary>
   [RequireComponent(typeof(Rigidbody))]
   public class GravityAffectedObject : MonoBehaviour
   {
      public bool AffectedByGlobalGravity { get; set; } = true;

      private Rigidbody rigid;

      private void Awake()
      {
         rigid = GetComponent<Rigidbody>();
      }

      /// <summary>
      /// 중력을 가합니다.
      /// </summary>
      /// <param name="dir">가할 방향</param>
      /// <param name="amount">가할 양</param>
      public virtual void Gravity(Vector3 dir, float amount)
      {
         rigid.AddForce(dir.normalized * amount, ForceMode.Force);
      }

      /// <summary>
      /// 중력을 가합니다.
      /// </summary>
      /// <param name="gravity">중력 인스턴스</param>
      public virtual void Gravity (GravityValue gravity)
      {
         rigid.AddForce(gravity._direction.normalized * gravity._force,
                        ForceMode.Force);
      }
   }
}