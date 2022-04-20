namespace Characters.Player.Move
{
   public class MovementVO
   {
      public int Forward;
      public int Backward;
      public int Left;
      public int Right;

      public MovementVO(int Forward, int Backward,
                        int Left,    int Right)
      {
         this.Forward   = Forward;
         this.Backward  = Backward;
         this.Left      = Left;
         this.Right     = Right;
      }
   }
}