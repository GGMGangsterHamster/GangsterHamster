using UnityEngine;

namespace _Core.Initialize
{
   abstract public class InitBase : MonoBehaviour
   {
      abstract public RunLevel RunLevel { get; }

      /// <summary>
      /// RunLevel 에 호출됨
      /// </summary>
      abstract public void Call();
   }
}