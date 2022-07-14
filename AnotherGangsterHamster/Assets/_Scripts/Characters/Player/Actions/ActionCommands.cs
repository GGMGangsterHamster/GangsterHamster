using _Core.Commands;

// 플레이어 이동 (점프, 웅크리기, 대쉬) 커멘드 패턴

namespace Characters.Player.Actions
{
   public class CrouchStart : Command
   {
      public CrouchStart(IActionable actionable)
      {
         Execute.AddListener(param => {
            actionable.CrouchStart();
         });
      }
      
   }

   public class CrouchEnd : Command
   {
      public CrouchEnd(IActionable actionable)
      {
         Execute.AddListener(param => {
            actionable.CrouchEnd();
         });
      }

   }

   public class Jump : Command
   {
      public Jump(IActionable actionable)
      {
         Execute.AddListener(param => {
            actionable.Jump();
         });
      }

   }

}