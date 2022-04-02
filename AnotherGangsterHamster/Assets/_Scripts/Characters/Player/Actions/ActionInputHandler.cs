using UnityEngine;
using System.Collections.Generic;
using _Core.Commands;


namespace Characters.Player.Actions
{
   public class ActionInputHander
   {
      public class ActionInputHandler : MonoBehaviour
      {
         public string _path = "KeyCodes/Actions";

         // IActionable 구체화 한 클레스
         private Actions _actions;
      }
   }
}