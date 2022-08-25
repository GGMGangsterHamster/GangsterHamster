using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Objects.InteractableObjects
{

    abstract public class @InteractableObjects : MonoBehaviour, ICallbacks
    {
        public List<Event> _callbacks
                 = new List<Event>();

        public List<Event> Callbacks => _callbacks;

        // 토글 방식 이벤트인지
        [field: SerializeField]
        public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false;

      [field: SerializeField]
      public bool Activated { get; set; } = false;

        [field: SerializeField]
        public bool MultipleInteractable { get; set; } = false;

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
            List<Event> addEvents = new();

            _callbacks.ForEach(@event => {
                @event.key.Split(',').ToList().ForEach(key => {
                    Event item = new Event();
                    item.key = key;
                    item.OnActive = @event.OnActive;
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
            if (!EventIsToggle && !MultipleInteractable && _objectCount > 0) return;

            Event callback = _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

            if (callback != null)
            {

                if (!EventIsToggle)
                {
                    ++_objectCount;
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
        /// <param name="noCount">가끔 Enter 메시지를 두번받고 Exit를 호출 하지 않는 경우가 있었기에 그 "유니티" 버그를 방지하기 위해서 생긴 놈 -햄-</param>
        protected void OnEventExit(GameObject other, bool noCount = false)
        {
            Event callback = _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

            if (callback != null)
            {
                --_objectCount;

                if (_objectCount > 0 && !noCount) return;

                _objectCount = 0;
                Activated = false;
                callback.OnDeactive?.Invoke(other);
            }
        }

        private void OnDisable()
        {
            if (_objectCount > 0)
            {
                _objectCount = 0;
                Activated = false;
            }
        }

    }
}