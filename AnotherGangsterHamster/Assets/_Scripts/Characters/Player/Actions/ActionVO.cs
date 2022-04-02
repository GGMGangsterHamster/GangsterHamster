namespace Characters.Player.Actions
{
   public class ActionVO
   {
      public int Dash;
      public int Jump;
      public int Crouch;

      public ActionVO(int Dash, int Jump, int Crouch)
      {
         this.Dash   = Dash;
         this.Jump   = Jump;
         this.Crouch = Crouch;
      }
   }
}