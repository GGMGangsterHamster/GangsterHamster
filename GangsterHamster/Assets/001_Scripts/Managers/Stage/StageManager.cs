using System;
using System.Collections.Generic;
using Objects.Stage;
using UnityEngine;



namespace Utils.Stage
{
   public enum Stage
   {
      STAGE_0_1,
      STAGE_0_2,
      STAGE_T1,
      STAGE_1_1,
      STAGE_1_2,
      END_OF_ENUM
   }


   public class StageManager : Singleton<StageManager>, ISingletonObject
   {
      public Action OnStageReset; // 스테이지 리셋 시 호출

      private const string STAGE_PATH = "Stages/"; // 스테이지 경로 (Resources 폴더 쪽)


      private readonly Dictionary<string, GameObject> _stageDictionary;       // 스테이지 오브젝트
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

            CheckAndAddToDictionary(_stageDictionary, key, stages[i]);

         }
      }
      private void SetStageBaseCoord()
      {
         StageBase[] stageBases =
                  UnityEngine.Object.FindObjectsOfType<StageBase>();
         
         if (stageBases.Length != _stageDictionary.Count) // 스테이지 수와 스테이지 베이스 수가 다른 경우
         {
            Logger.Log("StageManager > stage base object count mismatch" +
                      $"Expected {_stageDictionary.Count}" + 
                      $"but handled {stageBases.Length}",
                     LogLevel.Fatal);
            return;
         }

         for (int i = 0; i < stageBases.Length; ++i) // Dictionary 에 저장
         {
            string key = stageBases[i].name.Split(' ')[1]; // base 0-1, base T1

            CheckAndAddToDictionary(_stageBaseDictionary,
                                    key,
                                    stageBases[i].transform.position);
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
         if(!Contains.In<GameObject>(key, _stageDictionary))
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
         Contains.In<GameObject>(key, _stageDictionary, () => {

         });
      }

      private void CheckAndAddToDictionary<TValue>(object dictionary, string key, TValue value)
      {
         if (!(dictionary is Dictionary<string, TValue>))
         {
            Logger.Log("StageManager > Type mismatch", LogLevel.Fatal);
            return;
         }

         var dict = dictionary as Dictionary<string, TValue>;


         Contains.In<TValue>(key, dict, () =>
         {
            Logger.Log($"StageManager > Duplicate base name. Name:{key}",
                     LogLevel.Fatal);
         }, () =>
         {
            dict.Add(key, value);
         });
      }
   }
}