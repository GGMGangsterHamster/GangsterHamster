using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stage.Management
{

   public class StageManager : Singleton<StageManager>, ISingletonObject
   {
      public StageNames CurrentStage { get; private set; }

      protected StageManager()
      {
         CurrentStage = StageNames.NONE;
      }

      public void Load(StageNames target)
      {
         if (AvalibleToLoad(target))
         {
            SceneManager.LoadScene(target.ToString());
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