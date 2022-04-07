using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.StageObjects
{
   public class CollisionInteractableObject : MonoBehaviour
   {
      public List<CollisionCallback> _callbacks
               = new List<CollisionCallback>();

      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false;
      private bool _activated = false;

      private void Awake()
      {
         _activated = InitalActiveStatus;
      }

      #region Unity Collision Event
      private void OnCollisionEnter(Collision other)
      {
         CollisionEnterEvent(other.gameObject);
      }

      private void OnCollisionExit(Collision other)
      {
         if (!EventIsToggle)
            CollisionExitEvent(other.gameObject);
      }
      #endregion // Unity Collision Event

      /// <summary>
      /// 충돌 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void CollisionEnterEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => other.CompareTag(x.key));

         if (callback != null)
         {
            if (!EventIsToggle)
            {
               _activated = true;
               callback.OnActive?.Invoke(other);
               return;
            }

            _activated = !_activated;

            if (_activated)
               callback.OnActive?.Invoke(other);
            else
               callback.OnDeactive?.Invoke(other);
         }
      }

      /// <summary>
      /// Collision Exit 이벤트 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void CollisionExitEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => other.CompareTag(x.key));

         if (callback != null)
         {
            _activated = false;
            callback.OnDeactive?.Invoke(other);
         }
      }
   }
}