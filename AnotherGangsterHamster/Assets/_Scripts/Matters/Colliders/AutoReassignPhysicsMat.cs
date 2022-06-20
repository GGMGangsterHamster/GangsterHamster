using System.Collections.Generic;
using System.Linq;
using Matters.Gravity;
using UnityEngine;

namespace Matters.Colliders
{
   public class AutoReassignPhysicsMat : MonoBehaviour
   {
      private PhysicMaterial _wall;
      private PhysicMaterial _ground;
      private Collider[] _colliders;

      public Vector3 up = Vector3.zero;
      private Vector3 absUp;

      private void Start()
      {
         _wall = Resources.Load<PhysicMaterial>("Materials/Physics/Wall");
         _ground = Resources.Load<PhysicMaterial>("Materials/Physics/Ground");

         _colliders = GetComponents<Collider>();
         absUp = up.Abs();

         OnGravityChanged(GravityManager.GetGlobalGravity());

         GravityManager.AddGravityChangedListener(OnGravityChanged);
      }

      private void OnGravityChanged(GravityValue gravity)
      {
         PhysicMaterial mat = up.Multiply(gravity._direction).Abs() == absUp ? _ground : _wall;

         for (int i = 0; i < _colliders.Length; ++i)
         {
            _colliders[i].material = mat;
         }
      }


   }
}