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
                  Logger.Log($"{_instance.GetType()} Found more than one.", LogLevel.Error);
               }
            }
            else
            {
               Logger.Log($"FIXME: \"NAME\" Does not exist.", LogLevel.Fatal);
            }
            
         }
         return _instance;
      }
   }
}
