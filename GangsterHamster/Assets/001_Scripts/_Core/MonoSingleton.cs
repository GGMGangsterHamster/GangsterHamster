using System;
using System.Collections;
using UnityEngine;

abstract public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
   static private T _instance = null;
   static public T Instance
   {
      get
      {
         if (_instance == null)
         {
            T[] objs = FindObjectsOfType<T>();
            if (objs.Length > 0)
            {
               _instance = objs[0];
               if (objs.Length > 1)
               {
                  Logger.Log($"{_instance.GetType()} Found more than one.",
                           LogLevel.Error);
                  for (int i = 1; i < objs.Length; ++i) // 중복 오브젝트 삭제
                  {
                     Destroy(objs[i]);
                  }
                  GC.Collect();

               }

               DontDestroyOnLoad(_instance.gameObject);
            }
            else // 없으면 새로 만듬
            { 
               Logger.Log($"{typeof(T)} Does not exist, Creating.",
                        LogLevel.Warning);

               GameObject obj = new GameObject(typeof(T).ToString());
               obj.AddComponent<T>();

               _instance = obj.GetComponent<T>();
               DontDestroyOnLoad(_instance.gameObject);
            }
            
         }
         return _instance;
      }
   }
}
