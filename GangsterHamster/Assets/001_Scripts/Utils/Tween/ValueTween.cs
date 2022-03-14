using System;
using System.Collections;
using UnityEngine;

namespace Tween
{
    public static class ValueTween
    {
        public static void To(MonoBehaviour mono, Func<object> step, Func<object> execute, Func<bool> compare, Action callback = null) {
            mono.StartCoroutine(Interpolate(step, execute, compare, callback));
        }

        private static IEnumerator Interpolate(Func<object> step, Func<object> execute, Func<bool> compare, Action callback) {

            while(true) {
                if(compare()) break;
                yield return null;

                step();
            }

            callback?.Invoke();
        }
    }
}