using System;
using System.Collections.Generic;
using Objects.Stage;
using UnityEngine;



namespace Utils.Stage
{
   public class StageManager : Singleton<StageManager>, ISingletonObject
   {
      private const string STAGE_PATH = "Stages/";                               // 스테이지 경로 (Resources 폴더 쪽)
      private readonly Dictionary<string, GameObject> _stageDictionary;          // 스테이지 오브젝트
      private readonly Dictionary<string, Vector3>    _stageBaseDictionary;   // 스테이지 기준점 죄표

      #region Init

      protected StageManager()
      {
         _stageDictionary = new Dictionary<string, GameObject>();

         AddStageToDictionary();
         SetStageBaseCoord();
      }

      private void AddStageToDictionary()
      {
         GameObject[] stages = Resources.LoadAll<GameObject>(STAGE_PATH);
         
         for (int i = 0; i < stages.Length; ++i)
         {
            string key = stages[i].name.Split(' ')[1]; // stage "0-1", stage "T1"

            Contains<GameObject>(key, _stageDictionary, () => {
               Logger.Log($"StageManager > Duplicate stage name. Name:{key}",
                        LogLevel.Fatal);
            }, () => {
               _stageDictionary.Add(key, stages[i]);
            });

         }
      }
      private void SetStageBaseCoord()
      {
         StageBase[] stageBases =
                  UnityEngine.Object.FindObjectsOfType<StageBase>();
         
         if(stageBases.Length != _stageDictionary.Count)
         {
            Logger.Log("StageManager > stage base object count mismatch" +
                      $"Expected {_stageDictionary.Count}" + 
                      $"but handled {stageBases.Length}",
                     LogLevel.Fatal);
            return;
         }

         for (int i = 0; i < stageBases.Length; ++i) 
         {
            string key = stageBases[i].name.Split(' ')[1]; // base 0-1, base T1
            Contains<Vector3>(key, _stageBaseDictionary, () => {
               Logger.Log($"StageManager > Duplicate base name. Name:{key}",
                        LogLevel.Fatal);
            }, () => {
               _stageBaseDictionary.Add(key, stageBases[i].transform.position);
            });
         }


      }

      #endregion // Init

      /// <summary>
      /// 스테이지를 불러옵니다,
      /// </summary>
      /// <param name="key">stage "0-1", 0-2, T1 ...</param>
      /// <returns>null when there is no key named like handled key<returns>
      public GameObject GetStage(string key)
      {
         if(!Contains<GameObject>(key, _stageDictionary))
         {
            return null;
         }
         else
         {
            return _stageDictionary[key];
         }
      }
      
      /// <summary>
      /// 스테이지를 리셋합니다.
      /// </summary>
      /// <param name="key">리셋할 스테이지</param>
      public void ResetStage(string key)
      {
         Contains<GameObject>(key, _stageDictionary, () => {

         });
      }

      // 요약:
      //    dictionary 에 key 가 존재하는지 확인합니다.
      //
      // 반환:
      //    존재하지 않거나 오류가 발생한 경우 false
      private bool Contains<TValue>(string key,
                                    object dictionary, // fuck it
                                    Action onExists = null,
                                    Action onNull = null)
      {
         if (onNull == null)
         {
            onNull += () =>
            {
               Logger.Log($"StageManager > Stage: {key} does not exist",
                        LogLevel.Fatal);
            };
         }

         bool result;

         if(dictionary is Dictionary<string, TValue>)
         {
            var dict = dictionary as Dictionary<string, TValue>;
            result = dict.ContainsKey(key);
         }
         else
         {
            Logger.Log("StageManager > type mismatch: dictionary",
                     LogLevel.Fatal);
            return false;
         }

         if (result)
            onExists?.Invoke();
         else
            onNull();

         return result;

      }
   }
}