using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   public class CollisionStayInteractableObject : MonoBehaviour
   {

      public List<CollisionCallback> _callbacks
               = new List<CollisionCallback>();

      public Vector3 colPosition;

      private GameObject _curCollision;

      private void OnCollisionStay(Collision other)
      {
         colPosition = other.contacts[0].point;

         var obj = _callbacks.Find(x => (x.key == "")
               || other.gameObject.CompareTag(x.key));

         if (obj == null) return;

         _curCollision = other.gameObject;
         obj.OnActive?.Invoke(other.gameObject);
      }

      private void OnCollisionExit(Collision other)
      {
         if (_curCollision != other.gameObject) return;

         var obj = _callbacks.Find(x => (x.key == "")
               || other.gameObject.CompareTag(x.key));

         if (obj == null) return;

         obj.OnDeactive?.Invoke(other.gameObject);
      }
   }
}