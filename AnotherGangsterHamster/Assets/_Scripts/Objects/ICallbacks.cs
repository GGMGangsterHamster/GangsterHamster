using System.Collections.Generic;

namespace Objects
{
   public interface ICallbacks
   {
      public List<CollisionCallback> Callbacks { get; }
   }
}