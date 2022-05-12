using UnityEngine;

namespace Objects.StageObjects
{
   public class ActivateEvent : MonoBehaviour, ICollisionEventable
   {
      public void Active(GameObject other)
      {
         gameObject.SetActive(true);
      }

      public void Deactive(GameObject other)
      {
         gameObject.SetActive(false);
      }
   }
}