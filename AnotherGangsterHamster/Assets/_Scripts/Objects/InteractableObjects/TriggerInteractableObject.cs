using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
   public class TriggerInteractableObject : InteractableObjects
   {
      #region Unity Trigger Event
      private void OnTriggerEnter(Collider other)
      {
         OnEventTrigger(other.gameObject);
      }

      private void OnTriggerExit(Collider other)
      {
         if (EventIsToggle) return;

         OnEventExit(other.gameObject);
      }
      #endregion // Unity Trigger Event
   }
}