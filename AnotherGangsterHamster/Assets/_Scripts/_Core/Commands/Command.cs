namespace _Core.Commands
{
   abstract public class Command
   {
      abstract public void Execute(object param = null);

      public Command()
      {
         System.GC.KeepAlive(this);
      }
   }
}