using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
   public interface ICallbacks : IActivated
   {
      public List<Event> Callbacks { get; }
   }
}