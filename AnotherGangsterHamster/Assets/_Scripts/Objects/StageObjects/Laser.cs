using Characters.Damage;
using UnityEngine;


namespace Objects.StageObjects
{
   [RequireComponent(typeof(TriggerInteractableObject))]
   public class Laser : MonoBehaviour, ICollisionEventable
   {
      const string PLAYER = "PLAYER_BASE";

      [SerializeField] private int _damage = 100;

      public void Active(GameObject other)
      {
         if (other.TryGetComponent<IDamageable>(out var damageable))
         {
            damageable.Damage(_damage);
         }
      }

      public void Deactive(GameObject other) { }
   }
}
