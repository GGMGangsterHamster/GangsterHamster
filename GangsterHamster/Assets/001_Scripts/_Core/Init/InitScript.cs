// Init 시스템 (리눅스 원본)
// better than systemd
// more information: https://wiki.gentoo.org/wiki/Handbook:X86/Working/Initscripts

using System;
using System.Reflection;
using UnityEditor;
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

        public MonoScript script;

        MethodInfo _depend;
        MethodInfo _start;
        MethodInfo _stop;

        private object instance;

        public void Init()
        {
            Type type = script.GetClass();
            instance = Activator.CreateInstance(type);
            _depend = type.GetMethod("Depend");
            _start = type.GetMethod("Start");
            _stop = type.GetMethod("Stop");
        }

        /// <summary>
        /// 지정된 RunLevel 에 호출
        /// </summary>
        public void Depend(MonoBehaviour mono) => _depend.Invoke(instance, new object[] { mono });
        /// <summary>
        /// 의존성 해결<br/>
        /// 지정된 시작 호출 전 호출됨
        /// </summary>
        public void Call(object obj) => _start.Invoke(instance, new object[] { obj });
        /// <summary>
        /// 종료 시 호출
        /// </summary>
        public void Stop() => _stop.Invoke(instance, null);

    }
}