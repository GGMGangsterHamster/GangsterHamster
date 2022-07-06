using System.Collections.Generic;
using Objects.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   public class BothInteractableObject : Interactable, IActivated, ICallbacks
   {
      public List<CollisionCallback> _callbacks
               = new List<CollisionCallback>();
               
      public List<CollisionCallback> Callbacks => _callbacks;

      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false;


      private int _objectsCount = 0;

      // Collision에서 Normal 벡터를 빼내기 위해서 존재하는 변수
      public Vector3 colNormalVec;

      // 충돌 시의 Velocity
      public Vector3 colVelocity;

      // 충돌 지점
      public Vector3 colPosition;

      private ButtonCountRequirement _requirement;

      public bool canCollision = true;
      public bool canTrigger = false;

      private void Awake()
      {
         _activated = InitalActiveStatus;

         _requirement = GetComponent<ButtonCountRequirement>();
      }

      #region Unity Collision Event
      private void OnCollisionEnter(Collision other)
      {
         if (!canCollision) return;

         colNormalVec   = other.contacts[0].normal;
         colVelocity    = other.relativeVelocity;
         colPosition    = other.contacts[0].point;

         EnterEvent(other.gameObject);
      }

      private void OnTriggerEnter(Collider other)
      {
         if (!canTrigger) return;

         EnterEvent(other.gameObject);
      }

      private void OnCollisionExit(Collision other)
      {
         if (!canCollision || EventIsToggle) return;

         ExitEvent(other.gameObject);
      }

      private void OnTriggerExit(Collider other)
      {
         if (!canTrigger || EventIsToggle) return;

         ExitEvent(other.gameObject);
      }
      #endregion // Unity Collision Event

      /// <summary>
      /// 충돌 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void EnterEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            ++_objectsCount;

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
      public void ExitEvent(GameObject other)
      {
         CollisionCallback callback =
                  _callbacks.Find(x => (x.key == "") || other.CompareTag(x.key));

         if (callback != null)
         {
            --_objectsCount;

            if (_objectsCount < 0) return;

            _objectsCount = 0;
            _activated = false;
            callback.OnDeactive?.Invoke(other);
         }
      }


      public override void Interact()
      {
         if (_requirement == null || _requirement.Checked)
         {
            OnInteraction();
         }
      }

      public void OnInteraction()
      {
         EnterEvent(null);
      }


      public override void Focus()
      {
      }

      public override void DeFocus()
      {
      }

   }
}