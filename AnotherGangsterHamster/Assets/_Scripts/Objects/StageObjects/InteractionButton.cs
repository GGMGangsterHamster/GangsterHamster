using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Objects.Interaction;

namespace Objects.StageObjects
{
   public class InteractionButton : Interactable, IActivated
   {
      public UnityEvent<GameObject> OnActive;
      public UnityEvent<GameObject> OnDeactive;

      // 토글 방식 이벤트인지
      [field: SerializeField]
      public bool EventIsToggle { get; set; } = false;

      [field: SerializeField]
      public bool InitalActiveStatus { get; set; } = false; // 점프 OnStay

      private bool _activated = false;
      public bool Activated => _activated;

      private void Awake()
      {
         _activated = InitalActiveStatus;
      }

      #region Unity Collision Event

      public override void Interact()
      {
         Debug.Log("A");
         OnInteraction();
      }

      #endregion // Unity Collision Event

      /// <summary>
      /// 상호작용 시 호출됨
      /// </summary>
      public void OnInteraction()
      {
         _activated = !_activated;

         if (_activated)
            OnActive?.Invoke(null);
         else
         {
            if(EventIsToggle)   
               OnDeactive?.Invoke(null);
            else
               OnActive?.Invoke(null);
         }
      }


      public override void Focus()
      {
      }

      public override void DeFocus()
      {
      }
   }
}