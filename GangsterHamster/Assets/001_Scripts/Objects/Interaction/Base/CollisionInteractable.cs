using UnityEngine;

namespace Objects.Interactable
{
   
   abstract public class CollisionInteractable : MonoBehaviour
   {
      private void OnCollisionEnter(Collision other)
      {
         OnCollision(other.gameObject);
      }

      abstract protected void OnCollision(GameObject other);
   }
}