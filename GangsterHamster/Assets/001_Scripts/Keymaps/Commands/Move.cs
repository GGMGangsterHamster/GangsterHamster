using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Movement.Movements
{
    public class MoveFoward : Command
    {
        IMoveable _moveable;

        public MoveFoward(IMoveable moveable)
        {
            _moveable = moveable;
        }

        public override void Execute()
        {
            _moveable.MoveFoward();
        }
    }

    public class MoveBackword : Command
    {
        IMoveable _moveable;

        public MoveBackword(IMoveable moveable)
        {
            _moveable = moveable;
        }

        public override void Execute()
        {
            _moveable.MoveBackword();
        }
    }

    public class MoveLeft : Command
    {
        IMoveable _moveable;

        public MoveLeft(IMoveable moveable)
        {
            _moveable = moveable;
        }

        public override void Execute()
        {
            _moveable.MoveLeft();
        }
    }

    public class MoveRight : Command
    {
        IMoveable _moveable;

        public MoveRight(IMoveable moveable)
        {
            _moveable = moveable;
        }

        public override void Execute()
        {
            _moveable.MoveRight();
        }
    }

    public class Jump : Command
    {
        IJumpable _jumpable;

        public Jump(IJumpable jumpable)
        {
            _jumpable = jumpable;
        }

        public override void Execute()
        {
            _jumpable.Jump();
        }
    }

    public class MouseRightInput : Command
    {
        IMouseInputable _mouseInputable;
        public MouseRightInput(IMouseInputable mouseInputable)
        {
            _mouseInputable = mouseInputable;
        }
        public override void Execute()
        {
            _mouseInputable.MouseRightDown();
        }
    }

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
}