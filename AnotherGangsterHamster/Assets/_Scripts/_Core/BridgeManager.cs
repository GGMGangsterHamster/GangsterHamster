using Characters.Player.Bridge;
using UnityEngine;

namespace _Core
{
   public class BridgeManager : MonoBehaviour
   {
      private void Start()
      {
         if (Utils.CheckDuplicate<BridgeManager>())
         {
            Logger.Log("BridgeManager > Duplicate.",
               LogLevel.Error);
         }

         new ValueActionBridge();
      }
   }
}