using UnityEngine;

namespace Objects.StageObjects.CollisionEventable
{
   public class TriggerDoor : MonoBehaviour, ICollisionEventable
   {
      [SerializeField] private GameObject _door = null;

      public void Active(GameObject other)
      {
         _door.SetActive(false);
      }

      public void Deactive(GameObject other)
      {
         _door.SetActive(true);
      }
   }
}