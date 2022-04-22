namespace Characters.Player.Actions
{
   public interface IActionable
   {
      public void Interact();

      public void Jump();

      public void DashStart();
      public void DashEnd();

      public void CrouchStart();
      public void CrouchEnd();
   }
}