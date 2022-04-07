using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.GroundCheck
{
   [RequireComponent(typeof(BoxCollider))]
   public class GroundChecker : MonoBehaviour
   {
      public List<string> _tags = new List<string>();

      private IGroundCallback _callback = null;

      private void Awake()
      {
         _callback = GetComponentInChildren<IGroundCallback>();
      }

      private void OnTriggerEnter(Collider other)
      {
         if (_tags.Find(x => other.CompareTag(x)) != null)
            _callback?.OnGround();
      }

      private void OnTriggerExit(Collider other)
      {
         if (_tags.Find(x => other.CompareTag(x)) != null)
            _callback?.ExitGround();
      }
   }
}