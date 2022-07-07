using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Stages.Management
{
   public enum SceneEventType
   {
      UNLOADED,
      LOADED
   }

   public class StageManager : Singleton<StageManager>
   {
      public StageNames CurrentStage { get; set; }
      private string activatedCheckpointName = "";

      private Transform _playerTrm = null;
      private Transform PlayerTrm
      {
         get 
         {
            if (_playerTrm == null)
               _playerTrm = GameObject.FindWithTag("PLAYER_BASE").transform;

            return _playerTrm;
         }
      }

      public StageManager()
      {
         AddLoadedEvent((s, l) =>
         { // 체크포인트 등록
            LoadCheckpoint();
         });
      }

      private void LoadCheckpoint()
      {
         Debug.Log(activatedCheckpointName + " name");

         if (activatedCheckpointName != "")
         {
            GameObject checkpoint =
                     GameObject.Find(activatedCheckpointName);

            if (checkpoint == null) return; // 채크포인트를 찾을 수 없는 경우

            PlayerTrm.position = checkpoint.transform.position;

            activatedCheckpointName = "";
         }
      }

      public void Load(string target)
      {
         StageNames stage = (StageNames)Enum.Parse(typeof(StageNames), target);

         if (AvalibleToLoad(stage))
         {
            SceneManager.LoadScene(target);
            CurrentStage = stage;
            BackgroundMusic.Instance.StartBackgroundMusic();
            SaveStage(stage.ToString());
            GC.Collect();
         }

      }

      public void Reload()
      {
         if (AvalibleToLoad(CurrentStage))
         {
            SceneManager.LoadScene(CurrentStage.ToString());
            GC.Collect();
         }
      }

      public void ActivateCheckpoint(string name)
      {
         activatedCheckpointName = name;
      }

      public void AddLoadedEvent(UnityAction<Scene, LoadSceneMode> action)
      {
         SceneManager.sceneLoaded += action;
      }

      public void AddUnLoadedEvent(UnityAction<Scene> action)
      {
         SceneManager.sceneUnloaded += action;
      }

      public void RemoveLoadedEvent(UnityAction<Scene, LoadSceneMode> action)
      {
         SceneManager.sceneLoaded -= action;
      }

      public void RemoveUnLoadedEvent(UnityAction<Scene> action)
      {
         SceneManager.sceneUnloaded -= action;
      }

      public bool ExistsStage()
      {
          return File.Exists("stageData" + ".txt");
      }

      public void SaveStage(string stage)
      {
          StreamWriter sw = new StreamWriter("stageData" + ".txt");
          sw.WriteLine(stage);

          sw.Flush();
          sw.Close();
      }

      public void LoadStage()
      {
          if(File.Exists("stageData" + ".txt"))
          {
              StreamReader sr = new StreamReader("stageData" + ".txt");
              string data = sr.ReadLine();

              Load(data);
              sr.Close();
          }
      }

      /// <summary>
      /// 로딩 가능한 Scene 인지 확인함
      /// </summary>
      private bool AvalibleToLoad(StageNames target)
      {
         if (target == StageNames.NONE || target == StageNames.END_OF_ENUM)
         {
            Logger.Log("StageManager > Wrong target", LogLevel.Error);
            return false;
         }

         return true;
      }
   }

}