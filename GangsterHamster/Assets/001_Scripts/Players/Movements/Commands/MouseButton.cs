namespace Commands.Movement.Movements
{
   public class MouseRight : Command
   {
      IWeaponable _weaponable;

      public MouseRight(IWeaponable weaponable)
      {
         _weaponable = weaponable;
      }

      public override void Execute()
      {
         _weaponable.MouseRight();
      }
   }

   public class MouseLeft : Command
   {
      IWeaponable _weaponable;

      public MouseLeft(IWeaponable weaponable)
      {
         _weaponable = weaponable;
      }

      public override void Execute()
      {
         _weaponable.MouseLeft();
      }
   }
}