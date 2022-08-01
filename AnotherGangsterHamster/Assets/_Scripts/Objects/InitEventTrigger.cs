using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
   public class InitEventTrigger : MonoBehaviour
   {
      private void Start()
      {
         if (TryGetComponent<IEventable>(out var action))
         {
            action.Active(this.gameObject);
         }
         else
         {
            Logger.Log($"{gameObject.name} 에서 아무런 이벤트를 찾지 못함.",
                  LogLevel.Warning);
         }
      }
   }
}