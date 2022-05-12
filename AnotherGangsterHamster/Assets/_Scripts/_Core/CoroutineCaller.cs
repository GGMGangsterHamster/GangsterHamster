using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class CoroutineCaller : MonoSingleton<CoroutineCaller>
{
   /// <summary>
   /// 코루틴을 돌립니다.
   /// </summary>
   /// <param name="keepGoing">true 인 경우 계속 돌아감</param>
   /// <param name="wait">yield return (wait)</param>
   /// <param name="execute">Coroutine 이 실행할 것</param>
   public Coroutine Use(Func<bool> keepGoing,
                        Func<YieldInstruction> wait,
                        Action execute)
   {
      return StartCoroutine(
         Routine(keepGoing, wait, execute)
      );
   }

   IEnumerator Routine(Func<bool> keepGoing,
                       Func<YieldInstruction> wait,
                       Action execute)
   {
      while (keepGoing())
      {
         execute();
         yield return wait();
      }
   }
}