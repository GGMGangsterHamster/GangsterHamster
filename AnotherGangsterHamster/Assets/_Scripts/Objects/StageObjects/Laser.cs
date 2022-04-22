using Characters.Damage;
using UnityEngine;


namespace Objects.StageObjects
{
   [RequireComponent(typeof(TriggerInteractableObject), typeof(BoxCollider), typeof(LineRenderer))]
   public class Laser : MonoBehaviour, ICollisionEventable
   {
      const string PLAYER = "PLAYER_BASE";
      [SerializeField] private int _damage = 100;

      private LineRenderer _line;
      private BoxCollider  _collider;

      private void Awake()
      {
         _line = GetComponent<LineRenderer>();
         _collider = GetComponent<BoxCollider>();
      }

      private void Update()
      {
         if (UnityEngine.Physics.Raycast(transform.position,
                                         transform.forward,
                                         out var hit))
         {
            Vector3 targetPos = transform.InverseTransformPoint(hit.point);

            if (Utils.Compare(targetPos, Vector3.zero, 0.01f)) return;

            float length = (hit.point - transform.position).magnitude;
            
            _line.SetPosition(1, targetPos);

            Vector3 targetSize = _collider.size;
            targetSize.z = length;
            _collider.size = targetSize;

            Vector3 targetCenter = _collider.center;
            targetCenter.z = length / 2.0f;
            _collider.center = targetCenter;
         }
      }


      public void Active(GameObject other)
      {
         if (other.TryGetComponent<IDamageable>(out var damageable))
         {
            damageable.Damage(_damage);
         }
      }

      public void Deactive(GameObject other)
      {

      }
   }
}
