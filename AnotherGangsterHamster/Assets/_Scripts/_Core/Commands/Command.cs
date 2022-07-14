using UnityEngine.Events;

namespace _Core.Commands
{
   abstract public class Command
   {
      public UnityEvent<object> Execute;

      public Command()
      {
         Execute = new UnityEvent<object>();
         System.GC.KeepAlive(this);
      }
   }
}