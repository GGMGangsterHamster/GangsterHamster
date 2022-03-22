namespace Commands
{
   abstract public class WeaponCommand
   {
      abstract public void Left();
      abstract public void Right();
      abstract public void Reset();

      virtual public bool isActive { get; set; }

      public WeaponCommand()
      {
         System.GC.KeepAlive(this);
      }
   }
}
