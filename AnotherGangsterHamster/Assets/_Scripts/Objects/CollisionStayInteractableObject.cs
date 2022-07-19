using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   public class CollisionStayInteractableObject : MonoBehaviour
   {

      public List<Event> _callbacks
               = new List<Event>();

      public Vector3 colPosition;

      public bool isOn = true;

      private GameObject _curCollision;

      private void OnCollisionStay(Collision other)
      {
         if (!isOn) return;

         colPosition = other.contacts[0].point;

         var obj = _callbacks.Find(x => (x.key == "")
               || other.gameObject.CompareTag(x.key));

         if (obj == null) return;

         _curCollision = other.gameObject;
         obj.OnActive?.Invoke(other.gameObject);
      }

      private void OnCollisionExit(Collision other)
      {
         if (!isOn || _curCollision != other.gameObject) return;

         var obj = _callbacks.Find(x => (x.key == "")
               || other.gameObject.CompareTag(x.key));

         if (obj == null) return;

         obj.OnDeactive?.Invoke(other.gameObject);
      }
   }
}