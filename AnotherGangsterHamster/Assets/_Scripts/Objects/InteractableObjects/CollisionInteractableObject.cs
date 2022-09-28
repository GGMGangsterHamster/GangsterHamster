using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Objects.InteractableObjects
{
   public class CollisionInteractableObject : InteractableObjects
   {
      // Collision에서 Normal 벡터를 빼내기 위해서 존재하는 변수
      public Vector3 colNormalVec;

      // 충돌 시의 Velocity
      public Vector3 colVelocity;
      
      // 충돌 지점
      public Vector3 colPosition;
      
      #region Unity Collision Event
      private void OnCollisionEnter(Collision other)
      {
         colNormalVec   = other.contacts[0].normal;
         colVelocity    = other.relativeVelocity;
         colPosition    = other.contacts[0].point;

         OnEventTrigger(other.gameObject);
      }

      private void OnCollisionExit(Collision other)
      {
         if (EventIsToggle) return;

         OnEventExit(other.gameObject);
      }
      #endregion // Unity Collision Event
   }
}