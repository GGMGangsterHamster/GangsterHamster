using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Effects.Fullscreen;

namespace _Core.Initialize
{
   public class OpenRC : MonoBehaviour
   {
      [Header("Add InitScript here.")]
      public InitBase[] initScripts;

      private Dictionary<RunLevel, Action> _scriptAddedInitScripts;


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

         _scriptAddedInitScripts = new Dictionary<RunLevel, Action>();
      }

      private void Start()
      {
         FindAndExecute(RunLevel.GAME_START);
      }

      private void OnDestroy()
      {
         FindAndExecute(RunLevel.GAME_EXIT);
      }

      public void Add(RunLevel runLevel, Action script)
      {
         if (_scriptAddedInitScripts.ContainsKey(runLevel))
            _scriptAddedInitScripts[runLevel] += script;
         else
            _scriptAddedInitScripts.Add(runLevel, script);
      }

      private void FindAndExecute(RunLevel runLevel)
      {
         for (int i = 0; i < initScripts.Length; ++i)
         {
            if(initScripts[i].RunLevel == runLevel)
               initScripts[i].Call();
         }

         foreach (var initScript in _scriptAddedInitScripts)
         {
            if (initScript.Key == runLevel)
               initScript.Value();
         }
      }
   }
}