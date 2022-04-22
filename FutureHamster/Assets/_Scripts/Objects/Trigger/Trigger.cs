using System;
using UnityEngine;

namespace Objects.Trigger
{
   /// <summary>
   /// 트리거 오브젝트
   /// </summary>
   abstract public class Trigger : MonoBehaviour
   {
      protected virtual void OnTriggerEnter(Collider other)
      {
         OnTrigger(other.gameObject);
      }

      /// <summary>
      /// 물체가 들어왔을 시 호출됨
      /// </summary>
      /// <param name="other">작동시킨 오브젝트</param>
      abstract public void OnTrigger(GameObject other);

   }
}