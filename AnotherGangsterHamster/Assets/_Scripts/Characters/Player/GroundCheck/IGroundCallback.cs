namespace Characters.Player.GroundCheck
{
   public interface IGroundCallback
   {
      public void OnGround();
      public void StayGround();
      public void ExitGround();
   }
}
