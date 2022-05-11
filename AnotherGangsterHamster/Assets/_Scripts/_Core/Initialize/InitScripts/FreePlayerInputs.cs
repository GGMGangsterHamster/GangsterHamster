using Characters.Player;

namespace _Core.Initialize.InitScripts
{
   public class FreePlayerInputs : InitBase
   {
      public override RunLevel RunLevel => RunLevel.SCENE_LOAD;

      public override void Call()
      {
         PlayerStatus.Moveable   = true;
         PlayerStatus.Jumpable   = true;
         PlayerStatus.Crouchable = true;
      }
   }
}