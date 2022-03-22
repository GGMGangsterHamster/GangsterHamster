namespace Commands
{
   abstract public class Command
   {
      abstract public void Execute();

      public Command()
      {
         System.GC.KeepAlive(this);
      }
   }
}