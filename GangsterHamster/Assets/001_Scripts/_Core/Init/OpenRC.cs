using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenRC
{
    public class OpenRC : MonoSingleton<OpenRC>
    {
        [Header("Add InitScript ScriptableObject")]
        public List<InitScript> initScripts = new List<InitScript>();


        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            initScripts.FindAll(e => e._RunLevel == RunLevel.OnGameStart).ForEach(x => {
                x.Depend.Invoke(this);
                x.Start.Invoke();
            });
        }

        private void OnDestroy()
        {
            initScripts.ForEach(x => {
                x.Stop.Invoke();
            });
        }

        /// <summary>
        /// 이벤트를 발생시킵니다.
        /// </summary>
        public void SetFlag(RunLevel flag)
        {
            initScripts.FindAll(e => e._RunLevel == flag).ForEach(x => {
                x.Depend.Invoke(this);
                x.Start.Invoke();
            });
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