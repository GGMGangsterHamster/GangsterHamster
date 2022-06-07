using UnityEngine;
using System.Collections.Generic;

namespace Characters.Player.Move
{
   public class DeltaManager : MonoBehaviour
   {
      public List<MoveDelta> managedDelta;

      public MoveDelta Get(string id)
      {
         MoveDelta delta = managedDelta.Find(x => x.DeltaID == id);
         if (delta == null)
         {
            Logger.Log($"DeltaManager({gameObject.name})"
                     + $"Cannot found delta named {id}.", LogLevel.Error);
         }
         return delta; // TODO: velocityPool? addForce 할 때 마다 추가해서 다 더하면 되는
      }
   }
}