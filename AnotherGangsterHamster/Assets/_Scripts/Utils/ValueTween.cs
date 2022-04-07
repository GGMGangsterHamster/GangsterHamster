using System;
using System.Collections;
using UnityEngine;

namespace Tween
{
   static public class ValueTween
   {
      /// <summary>
      /// Tween 합니다.
      /// </summary>
      /// <param name="step">단계</param>
      /// <param name="compare">비교 (true 가 될 때 까지 실행)</param>
      /// <param name="callback">완료 시 호출</param>
      static public Coroutine To(MonoBehaviour mono,
                                 Action step,
                                 Func<bool> compare,
                                 Action callback = null)
      {
         return mono.StartCoroutine(
                  Interpolate(step, compare, callback)
               );
      }

      /// <summary>
      /// Tween 을 강제 종료합니다.
      /// </summary>
      /// <param name="target">종료할 Tween, 없다면 전부 종료시킴</param>
      static public void Stop(MonoBehaviour mono,
                              Coroutine target = null)
      {
         if (target != null)
         {
            mono.StopCoroutine(target);
         }
         else
         {
            mono.StopAllCoroutines();
         }
      }

      static private IEnumerator Interpolate(Action step,
                                             Func<bool> compare,
                                             Action callback)
      {

         while (true)
         {
            if (compare()) break;
            yield return null;

            step();
         }

         callback?.Invoke();
      }
   }
}