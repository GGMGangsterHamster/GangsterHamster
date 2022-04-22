using System;
using UnityEngine;

static partial class Utils
{
   /// <summary>
   /// 중복 검사 함수 (Mono)<br/>
   /// 중복된 오브젝트를 찾으면 self 를 없엠
   /// </summary>
   /// <typeparam name="T">중복 검사할 타입</typeparam>
   static public void FindDuplicate<T>(GameObject self) where T : Component
   {
      T[] objs = UnityEngine.Object.FindObjectsOfType<T>();

      if(objs.Length > 1)
      {
         Logger.Log($"Found duplicate {typeof(T)}.", LogLevel.Warning);
         MonoBehaviour.Destroy(self);
      }
   }
}