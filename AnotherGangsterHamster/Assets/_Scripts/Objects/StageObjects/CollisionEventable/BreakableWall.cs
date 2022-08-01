using UnityEngine;
using Objects.InteractableObjects;

namespace Objects.StageObjects.CollisionEventable
{
   [RequireComponent(typeof(CollisionInteractableObject))]
   public class BreakableWall : MonoBehaviour, IEventable
   {
      [field: SerializeField]
      public float ForceToBreak { get; set; }
      private float _sqrForceToBreak;

      [SerializeField] private GameObject _wall = null;

      private void Awake()
      {
         _sqrForceToBreak = ForceToBreak * ForceToBreak;
      }

      public void Active(GameObject other)
      {
         if (other.TryGetComponent<Rigidbody>(out var rigid))
         {
            if (rigid.velocity.sqrMagnitude >= _sqrForceToBreak)
            {
               _wall.SetActive(false);
            }
         }
      }

      public void Deactive(GameObject other) { } // Nothing
   }
}