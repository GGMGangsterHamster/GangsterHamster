using UnityEngine.Events;

namespace _Core.Commands
{
   abstract public class Command
   {
      public UnityEvent<object> Execute;

      public Command()
      {
         System.GC.KeepAlive(this);
      }
   }
}