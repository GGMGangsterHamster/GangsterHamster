using System.Collections;
using System.Collections.Generic;
using Obejcts.Utils;
using UnityEngine;

namespace Objects.Utils
{
   public class GroundChecker : MonoBehaviour
   {
      const string GROUND = "GROUND";

      private IGroundCallbackObject _callback;

      private int _targetLayer;

      private void Awake()
      {
         _targetLayer = LayerMask.GetMask(GROUND);
         _callback = GetComponentInChildren<IGroundCallbackObject>();

         Debug.Log(_targetLayer);
      }

      private void OnTriggerEnter(Collider other)
      {
         if ((1 << other.gameObject.layer) == _targetLayer)
         {
            _callback?.OnGround();
         }
      }

      private void OnTriggerStay(Collider other)
      {
         if ((1 << other.gameObject.layer) == _targetLayer)
         {
            Debug.Log("stay");
            _callback?.OnGround();
         }
      }

      private void OnTriggerExit(Collider other)
      {
         if ((1 << other.gameObject.layer) == _targetLayer)
         {
            _callback?.ExitGround();
         }
      }

      // private void Update()
      // {
      //    CheckGround();
      // }
   }
}