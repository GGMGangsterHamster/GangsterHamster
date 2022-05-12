using UnityEngine;

namespace Objects
{
   public class SetObjectParent : MonoBehaviour, ICollisionEventable
   {
      [SerializeField]
      private Transform _root = null;

      private void Awake()
      {
         if (_root == null)
         {
            _root = transform.root;
         }
      }

      public void Active(GameObject other)
      {
         // _root.SetParent(other.transform);
      }

      public void Deactive(GameObject other)
      {
         // _root.SetParent(null);
      }
   }
}