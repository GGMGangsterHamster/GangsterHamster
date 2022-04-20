using _Core.Commands;
using Characters.Player.Move;
using UnityEngine;

// 2차원 움직임 커멘드 패턴

namespace Characters.Player.Move
{
   public class MoveForward : Command
   {
      IMoveable _moveable;

      public MoveForward(IMoveable moveable)
               => _moveable = moveable;

      public override void Execute(object param = null) 
               => _moveable.MoveForward();
   }

   public class MoveBackward : Command
   {
      IMoveable _moveable;

      public MoveBackward(IMoveable moveable)
               => _moveable = moveable;

      public override void Execute(object param = null)
               => _moveable.MoveBackward();
   }

   public class MoveLeft : Command
   {
      IMoveable _moveable;

      public MoveLeft(IMoveable moveable)
               => _moveable = moveable;

      public override void Execute(object param = null)
               => _moveable.MoveLeft();
   }

   public class MoveRight : Command
   {
      IMoveable _moveable;

      public MoveRight(IMoveable moveable)
               => _moveable = moveable;

      public override void Execute(object param = null)
               => _moveable.MoveRight();
   }
}