using UnityEngine;

namespace Objects.Interactable
{
   
   abstract public class CollisionInteractable : MonoBehaviour
   {
      private void OnCollisionStay(Collision other)
      {
         OnCollision(other.gameObject);
      }

      abstract protected void OnCollision(GameObject other);
   }
}