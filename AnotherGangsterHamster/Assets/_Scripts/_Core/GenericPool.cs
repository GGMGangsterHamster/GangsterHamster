using System.Collections.Generic;
using UnityEngine;

namespace _Core
{
   public class GenericPool : Singleton<GenericPool>
   {
      private Dictionary<System.Type, List<Object>> _pool;

      public GenericPool()
      {
         _pool = new Dictionary<System.Type, List<Object>>();
      }

      /// <summary>
      /// 오브젝트를 가져옵니다.
      /// </summary>
      /// <typeparam name="T">가져올 타입</typeparam>
      public T Get<T>() where T : Component
      {
         if (!_pool.ContainsKey(typeof(T)))
         {
            Logger.Log($"Type: {typeof(T)} not found on pooled list",
                     LogLevel.Fatal);
            return null;
         }

         Object temp = _pool[typeof(T)]
                        .Find(x => (x as T).gameObject.activeSelf);

         if (temp == null)
            temp = Add<T>();

         return (temp as T);
      }

      /// <summary>
      /// 풀링할 오브젝트를 추가합니다.
      /// </summary>
      public void AddManagedObject<T>(
               int preInstantiatedObjectCount = 20)
               where T : Component
      {

         if (_pool.ContainsKey(typeof(T)))
         {
            Logger.Log($"Type: {typeof(T)} already pooled",
                     LogLevel.Warning);
            return;
         }

         _pool.Add(typeof(T), new List<Object>());

         for (int i = 0; i < preInstantiatedObjectCount; ++i)
         {
            Add<T>();
         }

      }

      /// <summary>
      /// _pool[T}List 에 추가
      /// </summary>
      private T Add<T>() where T : Component
      {
#if UNITY_EDITOR
         if(!_pool.ContainsKey(typeof(T)))
         {
            Logger.Log($"Type: {typeof(T)} not found on pooled list",
                     LogLevel.Fatal);
            return null;
         }
#endif

         T temp = Instantiate<T>();
         _pool[typeof(T)].Add(temp);
         return temp;
      }

      /// <summary>
      /// 새로운 GameObject 에 T 붙여서 Instantiate 함
      /// </summary>
      /// <typeparam name="T">컴포넌트</typeparam>
      private T Instantiate<T>() where T : Component
      {
         GameObject tempObject =
                     new GameObject(
                        typeof(T).ToString(),
                        typeof(T));

         tempObject.SetActive(false);

         return tempObject.GetComponent<T>();
      }
   }
}