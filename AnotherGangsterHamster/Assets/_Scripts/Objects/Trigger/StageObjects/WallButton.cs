using UnityEngine;
using UnityEngine.Events;

namespace Objects.Trigger
{
   public class WallButton : MonoBehaviour
   {
      private const string WEAPON = "WEAPON";

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
         if (other.CompareTag(WEAPON))
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