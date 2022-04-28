using Objects;
using UnityEngine;

namespace Matters.Velocity
{
   [RequireComponent(typeof(Rigidbody))]
   public class FreezeObject : MonoBehaviour, ICollisionEventable
   {
      public bool InitalStatus = false;

      // 프리즈 전 Velocity
      private Vector3   _pastVelocity;
      private Rigidbody _rigid;


      private void Awake()
      {
         _rigid = GetComponent<Rigidbody>();

         if (InitalStatus)
         {
            Active(this.gameObject);
         }
      }

      public void Active(GameObject other)
      {
         _rigid.constraints = RigidbodyConstraints.FreezeAll;
      }

      public void Deactive(GameObject other)
      {
         _rigid.constraints = RigidbodyConstraints.None;
      }
   }
}