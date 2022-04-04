using _Core.Commands;
using UnityEngine;

namespace Characters.Player.Mouse
{
   public class MouseX : Command
   {
      IMousedeltaRecvable _mouseDeltaRecvable;

      public MouseX(IMousedeltaRecvable mouseDeltaRecvable)
               => _mouseDeltaRecvable = mouseDeltaRecvable;

      public override void Execute(object param = null)
               => _mouseDeltaRecvable.OnMouseX(Input.GetAxis("Mouse X"));
   }

   public class MouseY : Command
   {
      IMousedeltaRecvable _mouseDeltaRecvable;

      public MouseY(IMousedeltaRecvable mouseDeltaRecvable)
               => _mouseDeltaRecvable = mouseDeltaRecvable;

      public override void Execute(object param = null)
               => _mouseDeltaRecvable.OnMouseY(Input.GetAxis("Mouse Y"));
   }
}