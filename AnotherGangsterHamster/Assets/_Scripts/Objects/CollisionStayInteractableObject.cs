using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
    public class CollisionStayInteractableObject : MonoBehaviour
    {
        public List<CollisionCallback> _callbacks
                 = new List<CollisionCallback>();

        private void OnCollisionStay(Collision other)
        {
            _callbacks.Find(x => (x.key == "") || other.gameObject.CompareTag(x.key))
                ?.OnActive?.
                Invoke(other.gameObject);
        }
    }
}