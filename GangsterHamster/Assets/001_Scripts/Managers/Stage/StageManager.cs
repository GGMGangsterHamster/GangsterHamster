using System;
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

      public StageManager()
      {
         CurrentStage = StageNames.NONE;
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