using _Core.Commands;

namespace Characters.Player.Actions
{
   public class CrouchStart : Command
   {
      IActionable _actionable;

      public CrouchStart(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.CrouchStart();
      
   }

   public class CrouchEnd : Command
   {
      IActionable _actionable;

      public CrouchEnd(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.CrouchEnd();

   }

   public class DashStart : Command
   {
      IActionable _actionable;

      public DashStart(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.DashStart();

   }

   public class DashEnd : Command
   {
      IActionable _actionable;

      public DashEnd(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.DashEnd();

   }

   public class Jump : Command
   {
      IActionable _actionable;

      public Jump(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.Jump();

   }
}