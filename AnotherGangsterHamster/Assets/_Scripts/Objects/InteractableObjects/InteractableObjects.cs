using UnityEngine;
using System.Linq;
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
      public bool Activated { get; set; }

      private int _objectCount = 0;

      protected virtual void Awake()
      {
         Activated = InitalActiveStatus;
         ParseTag();
      }

      /// <summary>
      /// ',' 로 Key 나눔
      /// 이제 무지성 복붙 대신 ATypeObject,BTypeObject 가 가능함
      /// </summary>
      private void ParseTag()
      {
         List<Event> deleteEvents = new();
         List<Event> addEvents    = new();

         _callbacks.ForEach(@event => {
            @event.key.Split(',').ToList().ForEach(key => {
               Event item      = new Event();
               item.key        = key;
               item.OnActive   = @event.OnActive;
               item.OnDeactive = @event.OnDeactive;
               
               addEvents.Add(item);
            });

            deleteEvents.Add(@event);
         });

         deleteEvents.ForEach(e => {
            _callbacks.Remove(e);
         });

         addEvents.ForEach(e => {
            _callbacks.Add(e);
         });
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
               Activated = true;
               callback.OnActive?.Invoke(other);
               return;
            }

            Activated = !Activated;

            if (Activated)
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
            Activated = false;
            callback.OnDeactive?.Invoke(other);
         }
      }



   }
}