using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.GroundCheck
{
   [RequireComponent(typeof(BoxCollider))]
   public class GroundChecker : MonoBehaviour
   {
      const string GROUND = "GROUND";

      private IGroundCallback _callback = null;
      private int _targetLayer;

      private void Awake()
      {
         _callback = GetComponentInChildren<IGroundCallback>();
      }

      private void OnTriggerEnter(Collider other)
      {
         if (other.CompareTag(GROUND))
         {
            _callback?.OnGround();
         }
      }

      private void OnTriggerStay(Collider other)
      {
         if (other.CompareTag(GROUND))
         {
            _callback?.OnGround();
         }
      }

      private void OnTriggerExit(Collider other)
      {
         if (other.CompareTag(GROUND))
         {
            _callback?.ExitGround();
         }
      }
   }
}