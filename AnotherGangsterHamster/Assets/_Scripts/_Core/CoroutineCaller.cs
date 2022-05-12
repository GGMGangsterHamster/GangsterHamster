using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class CoroutineCaller : MonoSingleton<CoroutineCaller>
{
   public Coroutine Use(Func<bool> keepGoing,
                        Action execute,
                        WaitForSeconds waitForSeconds = null)
   {
      return StartCoroutine(
         Routine(keepGoing,execute, waitForSeconds)
      );
   }

   IEnumerator Routine(Func<bool> keepGoing,
                       Action execute,
                       WaitForSeconds waitForSeconds = null)
   {
      while (keepGoing())
      {
         execute();
         yield return waitForSeconds;
      }
   }
}