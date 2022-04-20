namespace _Core.Initialize.InitScripts
{
   public class LockCursor : InitBase
   {
      public override RunLevel RunLevel => RunLevel.GAME_START;

      public override void Call()
      {
         Utils.LockCursor();
      }
   }
}