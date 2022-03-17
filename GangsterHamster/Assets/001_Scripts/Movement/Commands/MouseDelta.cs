using UnityEngine;

namespace Commands.Movement.Mouse
{
    #region MouseDelta
    public class MouseX : Command
    {
        IMouseDeltaRecvable _mouseDelta;

        public MouseX(IMouseDeltaRecvable mouseDeltaRecvable)
        {
            _mouseDelta = mouseDeltaRecvable;
        }

        public override void Execute()
        {
            float x = Input.GetAxis("Mouse X");
            _mouseDelta.OnMouseX(x);

        }
    }

    public class MouseY : Command
    {
        IMouseDeltaRecvable _mouseDelta;

        public MouseY(IMouseDeltaRecvable mouseDeltaRecvable)
        {
            _mouseDelta = mouseDeltaRecvable;
        }

        public override void Execute()
        {
            float y = Input.GetAxis("Mouse Y");
            _mouseDelta.OnMouseY(y);

        }
    }
    #endregion // MouseDelta
}