using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
   public class TriggerStayInteractableObject : InteractableObjects
   {
      private void OnTriggerStay(Collider other)
      {
         OnEventTrigger(other.gameObject);
      }

      private void OnTriggerExit(Collider other)
      {
         OnEventExit(other.gameObject);
      }
   }
}