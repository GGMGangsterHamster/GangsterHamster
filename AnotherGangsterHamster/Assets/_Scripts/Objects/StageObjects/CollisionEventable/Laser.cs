using Characters.Damage;
using Objects.InteractableObjects;
using UnityEngine;
using UnityEngine.Events;


namespace Objects.StageObjects.CollisionEventable
{
   [RequireComponent(typeof(TriggerInteractableObject), typeof(BoxCollider), typeof(LineRenderer))]
   public class Laser : MonoBehaviour, IEventable
   {
      [Header("충돌 시 월드 좌표를 넘겨주며 호출됨")]
      public UnityEvent<Vector3> OnCollision;

      const string PLAYER = "PLAYER_BASE";
      [SerializeField] private int _damage = 100;
      [SerializeField] LayerMask _ignoreMe;
      
      private LineRenderer _line;
      private BoxCollider  _collider;


      private void Awake()
      {
         _line = GetComponent<LineRenderer>();
         _collider = GetComponent<BoxCollider>();
         _ignoreMe = ~_ignoreMe;
      }

      private void Update()
      {
         if (UnityEngine.Physics.Raycast(transform.position,
                                         transform.forward,
                                         out var hit,
                                         Mathf.Infinity,
                                         _ignoreMe & LayerMask.NameToLayer("ONLYCOLPLAYER")))
         {
            Vector3 targetPos = transform.InverseTransformPoint(hit.point);
            OnCollision?.Invoke(hit.point);

            if (Utils.Compare(targetPos, Vector3.zero, 0.01f)) return;

            float length = (hit.point - transform.position).magnitude + 0.05f;
            
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
