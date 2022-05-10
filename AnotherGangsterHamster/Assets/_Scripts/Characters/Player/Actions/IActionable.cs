using System;
using UnityEngine;

namespace Characters.Player.Actions
{
   public interface IActionable
   {
      public void Interact(Action<Transform> onPickup = null);

      public void Jump();

      public void DashStart();
      public void DashEnd();

      public void CrouchStart();
      public void CrouchEnd();
   }
}