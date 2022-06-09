using _Core.Commands;

// 플레이어 이동 (점프, 웅크리기, 대쉬) 커멘드 패턴

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

   public class Jump : Command
   {
      IActionable _actionable;

      public Jump(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.Jump();

   }

}