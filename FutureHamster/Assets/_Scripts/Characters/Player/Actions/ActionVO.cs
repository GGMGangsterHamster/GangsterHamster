namespace Characters.Player.Actions
{
   public class ActionVO
   {
      public int Interact;
      public int Dash;
      public int Jump;
      public int Crouch;

      public ActionVO(int Interact, int Dash, int Jump, int Crouch)
      {
         this.Interact  = Interact;
         this.Dash      = Dash;
         this.Jump      = Jump;
         this.Crouch    = Crouch;
      }
   }
}