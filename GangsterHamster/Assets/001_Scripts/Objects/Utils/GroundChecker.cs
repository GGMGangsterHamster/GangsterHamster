using System.Collections;
using System.Collections.Generic;
using Obejcts.Utils;
using UnityEngine;

namespace Objects.Utils
{
   public class GroundChecker : MonoBehaviour
   {
      const string GROUND = "GROUND";

      [Header("바닥과 플레이어 거리")]
      [SerializeField] private float _groundDistance;

      private IGroundCallbackObject _callback;

      private int _targetLayer;

      private void Awake()
      {
         _targetLayer = LayerMask.GetMask(GROUND);
         _callback = GetComponentInChildren<IGroundCallbackObject>();
      }

      public bool CheckGround()
      {
         Vector3 pos = transform.position;
         pos.y += 0.05f;
         bool res = Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out RaycastHit hit, _groundDistance, _targetLayer);

         switch(res)
         {
            case true:  _callback?.ExitGround();   break;
            case false: _callback?.OnGround();     break;
         }

         return res;
      }

      private void Update()
      {
         CheckGround();
      }
   }
}