namespace _Core.Initialize.InitScripts
{
   public class LockCursor : InitBase
   {
      public override RunLevel RunLevel => RunLevel.SCENE_LOAD;

      public override void Call()
      {
         Utils.LockCursor();
      }
   }
}