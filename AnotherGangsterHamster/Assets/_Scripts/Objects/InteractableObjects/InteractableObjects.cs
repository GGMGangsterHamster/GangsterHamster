using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Objects.InteractableObjects
{

   public class @InteractableObjects : MonoBehaviour, ICallbacks
   {
      public List<Event> _callbacks
               = new List<Event>();

      public List<Event> Callbacks => _callbacks;

      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false;

      private bool _activated = false;
      public bool Activated { get; set; }

      private int _objectCount = 0;

      protected virtual void Awake()
      {
         _activated = InitalActiveStatus;
      }

      /// <summary>
      /// Active 시 호출
      /// </summary>
      /// <param name="other">Trigger 한 GameObject</param>
      protected void OnEventTrigger(GameObject other)
      {
         Event callback = _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            ++_objectCount;

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
      /// Deactive 시 호출
      /// </summary>
      /// <param name="other">Trigger 한 GameObject</param>
      protected void OnEventExit(GameObject other)
      {
         Event callback = _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            --_objectCount;

            if (_objectCount > 0) return;

            _objectCount = 0;
            _activated = false;
            callback.OnDeactive?.Invoke(other);
         }
      }



   }
}