using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager.sceneLoaded

namespace OpenRC
{
    public class OpenRC : MonoSingleton<OpenRC>
    {
        [Header("Add InitScript ScriptableObject Here.", order = 0)]
        [Space(-10, order = 1)]
        [Header("   ## Execution order not guaranteed ##", order = 2)]
        public List<InitScript> initScripts = new List<InitScript>();


        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            initScripts.ForEach(x => x.Init());

            FindAndExecute(RunLevel.OnGameStart);

            SceneManager.sceneLoaded += (scene, mode) => {
                FindAndExecute(RunLevel.OnSceneLoad, scene.name);
            }; // SceneManager.sceneLoaded

            SceneManager.sceneUnloaded += (scene) => {
                FindAndExecute(RunLevel.OnSceneUnLoad, scene.name);
            }; // SceneManager.sceneUnloaded
        }

        private void OnDestroy()
        {
            FindAndExecute(RunLevel.OnGameExit);

            initScripts.ForEach(x => {
                x.Stop();
            });
        }

        /// <summary>
        /// runLevel 에 등록된 InitScript 를 찾은 다음 argument 를 메개변수로 호출함
        /// </summary>
        private void FindAndExecute(RunLevel runLevel, object argument = null)
        {
            var list = initScripts.FindAll(e => e._RunLevel == runLevel);
            int i = list.Count;
            
            while(--i >= 0) {
                list[i].Depend(this);
                list[i].Call(argument);
            }
        }

        /// <summary>
        /// initScript 를 runLevel 에 실행되게 추가합니다.
        /// </summary>
        public int Add(InitScript initScript)
        {
            if(initScripts.Contains(initScript)) {
                Log.Debug.Log($"OpenRC > {initScript._Name} is already added to RunLevel {initScripts.Find(x => x == initScript)._RunLevel}", Log.LogLevel.Error);
                return 1;
            }

            initScripts.Add(initScript);
            Log.Debug.Log($"OpenRC > Added {initScript._Name} to RunLevel {initScript._RunLevel}");
            return 0;
        }

        /// <summary>
        /// initScript 를 runLevel 실행에서 제외합니다.
        /// </summary>
        public int Del(InitScript initScript)
        {
            if (!initScripts.Contains(initScript)) {
                Log.Debug.Log($"OpenRC > Cannot find {initScript._Name} at Runlevel {initScript._RunLevel}", Log.LogLevel.Error);
                return 1;
            }

            initScripts.Remove(initScript);
            Log.Debug.Log($"OpenRC > Removed {initScript._Name} at RunLevel {initScript._RunLevel}");
            return 0;
        }
    }
}