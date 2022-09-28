using System;
using _Core.Commands;
using UnityEngine;

// 마우스 커멘드 패턴

namespace Characters.Player.Mouse
{
   public class MouseX : Command
   {
      public MouseX(IMousedeltaRecvable mouseDeltaRecvable)
      {
         Execute.AddListener(param => {
            mouseDeltaRecvable.OnMouseX(Input.GetAxis("Mouse X"), (Action<float>)param);
         });
      }
   }

   public class MouseY : Command
   {
      public MouseY(IMousedeltaRecvable mouseDeltaRecvable)
      {
         Execute.AddListener(param => {
            mouseDeltaRecvable.OnMouseY(Input.GetAxis("Mouse Y"));
         });
      }
   }
}