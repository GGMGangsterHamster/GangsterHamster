using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace _Core.Initialize
{
   public class OpenRC : MonoBehaviour
   {
      [Header("Add InitScript here.")]
      public InitBase[] _initScripts = new InitBase[0];


      private void Awake()
      {
         Utils.FindDuplicate<OpenRC>(this.gameObject);
         DontDestroyOnLoad(this.gameObject);

         SceneManager.sceneLoaded += (scene, mode) =>
         {
            FindAndExecute(RunLevel.SCENE_LOAD);
         };

         SceneManager.sceneUnloaded += (scene) =>
         {
            FindAndExecute(RunLevel.SCENE_UNLOAD);
         };
      }

      private void Start()
      {
         FindAndExecute(RunLevel.GAME_START);
      }

      private void OnDestroy()
      {
         FindAndExecute(RunLevel.GAME_EXIT);
      }

      private void FindAndExecute(RunLevel runLevel)
      {
         for (int i = 0; i < _initScripts.Length; ++i)
         {
            if(_initScripts[i]._runLevel == runLevel)
               _initScripts[i].Call();
         }
      }
   }
}