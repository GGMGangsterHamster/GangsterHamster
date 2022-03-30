using System;
using Movement.Delta;
using UnityEngine;

namespace Objects.Interactable
{
   public class Treadmill : CollisionInteractable
   {
      public Vector3 direction;

      protected override void OnCollision(GameObject other)
      {
         DeltaMoveable delta = other.GetComponent<DeltaMoveable>();
         delta.AddRawDelta(direction);
      }
   }
}