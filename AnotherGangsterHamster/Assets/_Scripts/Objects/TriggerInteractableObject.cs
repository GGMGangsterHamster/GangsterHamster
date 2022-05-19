using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   public class TriggerInteractableObject : MonoBehaviour, IActivated
   {
      public List<CollisionCallback> _callbacks
               = new List<CollisionCallback>();

      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false;

      [field: SerializeField]
      public bool MultipleCollisionable { get; set; } = false;


      [HideInInspector] public bool _activated = false;
      public bool Activated => _activated;

      private Collider _curTriggerObj;


      private void Awake()
      {
         _activated = InitalActiveStatus;
      }

      #region Unity Trigger Event
      private void OnTriggerEnter(Collider other)
      {
         if (!MultipleCollisionable && _curTriggerObj != null) return;
         _curTriggerObj = other;

         TriggerEnterEvent(other.gameObject);
      }

      private void OnTriggerExit(Collider other)
      {
         if (_curTriggerObj != other || EventIsToggle) return;

         TriggerExitEvent(other.gameObject);
         _curTriggerObj = null;
      }
      #endregion // Unity Trigger Event

      /// <summary>
      /// 충돌 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void TriggerEnterEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            if (!EventIsToggle)
            {
               _activated = true;
               callback.OnActive?.Invoke(other);
               return;
            }

            _activated = !_activated; // TODO: 문에 이거 걸어야 힘

            if (_activated)
               callback.OnActive?.Invoke(other);
            else
               callback.OnDeactive?.Invoke(other);
         }
      }

      /// <summary>
      /// Trigger Exit 이벤트 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void TriggerExitEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            _activated = false;
            callback.OnDeactive?.Invoke(other);
         }
      }
   }
}