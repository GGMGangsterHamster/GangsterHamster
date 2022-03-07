using System.Collections.Generic;
using Objects.Init;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager.sceneLoaded

namespace OpenRC
{
    public class OpenRC : MonoSingleton<OpenRC>
    {
        [Header("Add InitScript ScriptableObject Here.", order = 0)]
        [Space(-10, order = 1)]
        [Header("   ## Execution order not guaranteed ##", order = 2)]
        public List<InitBase> initScripts = new List<InitBase>();

        // private List<GameObject> dummyInstanceList = new List<GameObject>(); // 초기화 스크립트 용


        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            // Object obj = Resources.Load("_InitScripts/Camera/AttachCameraToPlayer");




            // InitBase a = Resources.Load<InitBase>("_InitScripts/Camera/AttachCameraToPlayer");
            // Debug.Log(a == null);
            // Debug.Log(a.GetType());
            // InitBase code = (res as InitBase);
            // code.Depend(this);


            // initScripts.ForEach(x => x.Init());

            FindAndExecute(RunLevel.OnGameStart);

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                FindAndExecute(RunLevel.OnSceneLoad, scene.name);
            }; // SceneManager.sceneLoaded

            SceneManager.sceneUnloaded += (scene) =>
            {
                FindAndExecute(RunLevel.OnSceneUnLoad, scene.name);
            }; // SceneManager.sceneUnloaded
        }

        private void OnDestroy()
        {
            FindAndExecute(RunLevel.OnGameExit);

            initScripts.ForEach(x =>
            {
                x.Stop();
            });
        }

        /// <summary>
        /// runLevel 에 등록된 InitScript 를 찾은 다음 argument 를 메개변수로 호출함
        /// </summary>
        private void FindAndExecute(RunLevel runLevel, object argument = null)
        {
            var list = initScripts.FindAll(e => e.RunLevel == runLevel);
            int i = list.Count;

            while (--i >= 0)
            {
                list[i].Depend(this);
                list[i].Call(argument);
            }
        }

        /// <summary>
        /// initScript 를 runLevel 에 실행되게 추가합니다.
        /// </summary>
        public int Add(InitBase initScript)
        {
            if (initScripts.Contains(initScript))
            {
                Logger.Log($"OpenRC > {initScript.Name} is already added to RunLevel {initScripts.Find(x => x == initScript).RunLevel}", LogLevel.Error);
                return 1;
            }

            initScripts.Add(initScript);
            Logger.Log($"OpenRC > Added {initScript.Name} to RunLevel {initScript.RunLevel}");
            return 0;
        }

        /// <summary>
        /// initScript 를 runLevel 실행에서 제외합니다.
        /// </summary>
        public int Del(InitBase initScript)
        {
            if (!initScripts.Contains(initScript))
            {
                Logger.Log($"OpenRC > Cannot find {initScript.Name} at Runlevel {initScript.RunLevel}", LogLevel.Error);
                return 1;
            }

            initScripts.Remove(initScript);
            Logger.Log($"OpenRC > Removed {initScript.Name} at RunLevel {initScript.RunLevel}");
            return 0;
        }
    }
}