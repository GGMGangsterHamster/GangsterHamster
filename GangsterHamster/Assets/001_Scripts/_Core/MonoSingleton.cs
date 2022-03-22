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
            CheckDuplicate();
            
            T[] objs = FindObjectsOfType<T>();

            if (objs.Length > 0)
            {
               _instance = objs[0];
               _instance.name = typeof(T).ToString();

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

   static protected void CheckDuplicate()
   {
      T[] arr = FindObjectsOfType<T>();

      if (arr.Length > 1)
      {
         Logger.Log($"{_instance.GetType()} Found more than one.",
                  LogLevel.Error);

         for (int i = 0; i < arr.Length; ++i)
         {
            if (arr[i].name != typeof(T).ToString())
               Destroy(arr[i]);
         }

         GC.Collect();
      }
   }
}
