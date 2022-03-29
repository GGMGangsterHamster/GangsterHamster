using UnityEngine;

namespace Objects.Destroyable
{
   abstract public class Destroyable : MonoBehaviour
   {
      /// <summary>
      /// 부서질 시
      /// </summary>
      abstract protected void Die();

      /// <summary>
      /// 데미지 처리
      /// </summary>
      /// <param name="other">충돌한 게임오브젝트</param>
      abstract protected void OnDamage(GameObject other);

      private void OnCollisionEnter(Collision other)
      {
         OnDamage(other.gameObject);
      }
   }
}
