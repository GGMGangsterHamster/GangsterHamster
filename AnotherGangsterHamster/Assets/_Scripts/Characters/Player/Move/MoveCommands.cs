using _Core.Commands;

// 2차원 움직임 커멘드 패턴

namespace Characters.Player.Move
{
   public class MoveForward : Command
   {
      public MoveForward(IMoveable moveable)
      {
         Execute.AddListener(param => {
            moveable.MoveForward();
         });
      }
   }

   public class MoveBackward : Command
   {
      public MoveBackward(IMoveable moveable)
      {
         Execute.AddListener(param => {
            moveable.MoveBackward();
         });
      }
   }

   public class MoveLeft : Command
   {
      public MoveLeft(IMoveable moveable)
      {
         Execute.AddListener(param => {
            moveable.MoveLeft();
         });
      }
   }

   public class MoveRight : Command
   {
      public MoveRight(IMoveable moveable)
      {
         Execute.AddListener(param => {
            moveable.MoveRight();
         });
      }
   }
}