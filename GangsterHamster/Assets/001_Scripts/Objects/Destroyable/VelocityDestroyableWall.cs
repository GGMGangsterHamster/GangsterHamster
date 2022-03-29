using UnityEngine;

namespace Objects.Destroyable
{
   public class VelocityDestroyableWall : Destroyable
   {
      [SerializeField] Vector3 _forceToBreak = Vector3.zero;

      protected override void Die()
      {
         gameObject.SetActive(false);
      }

      protected override void OnDamage(GameObject other)
      {
         if(other.CompareTag("PLAYER_BASE"))
         {
            Rigidbody rigid = other.GetComponent<Rigidbody>();

            Debug.Log(rigid.velocity);

            if (rigid.velocity.x >= _forceToBreak.x)
               Die();
            else if (rigid.velocity.z >= _forceToBreak.z)
               Die();

         }
      }
   }
}