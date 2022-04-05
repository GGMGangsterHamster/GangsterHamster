using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.Trigger
{
   public class CollisionInteractableObject : MonoBehaviour
   {
      [SerializeField] private List<string> _targetTags
               = new List<string>();

      public UnityEvent OnActive;
      public UnityEvent OnDeactive;


      [field: SerializeField]
      public bool InitalActiveStatus { get; set; }
      private bool _activated = false;

      private void Awake()
      {
         _activated = InitalActiveStatus;
      }

      public void CollisionEvent(GameObject other)
      {
         if (_targetTags.Find(x => x == other.tag) != null)
         {
            _activated = !_activated;

            if (_activated)
               OnButtonActivate();
            else
               OnButtonDeactive();
         }
      }

      private void OnButtonActivate()
      {
         OnActive?.Invoke();
      }

      private void OnButtonDeactive()
      {
         OnDeactive?.Invoke();
      }
   }
}