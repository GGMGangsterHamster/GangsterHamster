using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.StageObjects
{
   public class CollisionInteractableObject : MonoBehaviour
   {
      [Header("충돌 이벤트 발생 허용할 테그들")]
      [SerializeField] private List<string> _targetTags
               = new List<string>();

      public UnityEvent<GameObject> OnActive;
      public UnityEvent<GameObject> OnDeactive;


      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = true;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; }
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
         if(!EventIsToggle)
            CollisionExitEvent(other.gameObject);
      }
      #endregion // Unity Collision Event
      
      /// <summary>
      /// 충돌 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void CollisionEnterEvent(GameObject other)
      {
         if (_targetTags.Find(x => x == other.tag) != null)
         {
            if(!EventIsToggle)
            {
               _activated = true;
               OnActivate(other);
               return;
            }

            _activated = !_activated;

            if (_activated)
               OnActivate(other);
            else
               OnDeactivate(other);
         }
      }

      /// <summary>
      /// Collision Exit 이벤트 시 호출됨
      /// </summary>
      /// <param name="other">충돌한 GameObject</param>
      public void CollisionExitEvent(GameObject other)
      {
         if (_targetTags.Find(x => x == other.tag) != null)
         {
            _activated = false;
            OnDeactivate(other);
         }
      }

      private void OnActivate(GameObject other)
      {
         OnActive?.Invoke(other);
      }

      private void OnDeactivate(GameObject other)
      {
         OnDeactive?.Invoke(other);
      }
   }
}