// Init 시스템 (리눅스 원본)
// better than systemd
// more information: https://wiki.gentoo.org/wiki/Handbook:X86/Working/Initscripts

using System;
using System.Linq;
using System.Reflection;
using Objects.Init;
using UnityEngine;
using UnityEngine.Events;

namespace OpenRC
{
    /// <summary>
    /// OpenRC 이용을 위한 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "InitScript", menuName = "OpenRC Init Script")]
    public class InitScript : ScriptableObject
    {
        public RunLevel _RunLevel = RunLevel.OnGameStart;
        public string _Name = "";

        // public UnityEngine.Object script;

        // MethodInfo _depend;
        // MethodInfo _start;
        // MethodInfo _stop;
        Action _depend;
        Action _start;
        Action _stop;

        private object instance;

        public Component script;
        public string path = "_InitScripts/Camera";
        IInitBase init;

        public void Init()
        {
            // init = a.GetComponent<IInitBase>();



            // Debug.Log(type);
            // instance = Activator.CreateInstance(type);

            // type.GetMethods().ToList().ForEach(e => {
            //     Debug.Log(e);
            // });

            // _depend = type.GetMethod("Depend");
            // _start = type.GetMethod("Call");
            // _stop = type.GetMethod("Stop");
        }

        /// <summary>
        /// 지정된 RunLevel 에 호출
        /// </summary>
        public void Depend(MonoBehaviour mono) => init.Depend(mono);
        // public void Depend(MonoBehaviour mono) => _depend.Invoke(instance, new object[] { mono });
        /// <summary>
        /// 의존성 해결<br/>
        /// 지정된 시작 호출 전 호출됨
        /// </summary>
        public void Call(object obj) => init.Call(obj);
        // public void Call(object obj) => _start.Invoke(instance, new object[] { obj });
        /// <summary>
        /// 종료 시 호출
        /// </summary>
        public void Stop() => init.Stop();
        // public void Stop() => _stop.Invoke(instance, null);

    }
}





