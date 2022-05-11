namespace Commands.Movement.Movements
{
   public class DashStart : Command
   {
      IMoveable _moveable;

      public DashStart(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.DashStart();
      }
   }

   public class DashStop : Command
   {
      IMoveable _moveable;

      public DashStop(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.DashStop();
      }
   }
}