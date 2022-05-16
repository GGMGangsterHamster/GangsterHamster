using System;
using Stages.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Core.Initialize.InitScripts
{   
   public class SetCurrentScene : InitBase
   {
      public override RunLevel RunLevel => RunLevel.SCENE_LOAD;

      public override void Call()
      {
         StageManager.Instance.CurrentStage =
            (StageNames)Enum.Parse(typeof(StageNames),
                                   SceneManager.GetActiveScene().name);
      }
   }
}