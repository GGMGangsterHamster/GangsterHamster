using UnityEngine;

namespace _Core.Initialize
{
   abstract public class InitBase : MonoBehaviour
   {
      public RunLevel _runLevel;

      /// <summary>
      /// RunLevel 에 호출됨
      /// </summary>
      abstract public void Call();
   }
}