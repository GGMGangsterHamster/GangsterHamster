using System;
using System.Collections.Generic;
using Objects.Checkpoint;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Stage.Management
{
   public enum SceneEventType
   {
      UNLOADED,
      LOADED
   }

   public class StageManager : Singleton<StageManager>, ISingletonObject
   {
      public StageNames CurrentStage { get; private set; }
      private string activatedCheckpointName = "";

      public StageManager()
      {
         CurrentStage = StageNames.Stage_1_1;

         AddLoadedEvent((s, l) => { // 체크포인트 등록
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

            if(checkpoint == null) return; // 채크포인트를 찾을 수 없는 경우

            GameManager.Instance.Player.transform.position =
                     checkpoint.transform.position;

            activatedCheckpointName = "";
         }
      }

      public void Load(StageNames target)
      {
         if (AvalibleToLoad(target))
         {
            SceneManager.LoadScene(target.ToString());
            CurrentStage = target;
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