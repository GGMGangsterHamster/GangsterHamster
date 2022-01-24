// Init 시스템 (리눅스 원본)
// better than systemd
// more information: https://wiki.gentoo.org/wiki/Handbook:X86/Working/Initscripts

using System;
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

        /// <summary>
        /// 의존성 해결<br/>
        /// 지정된 시작 호출 전 호출됨
        /// </summary>
        public UnityEvent<MonoBehaviour> Depend;

        /// <summary>
        /// 지정된 RunLevel 에 호출
        /// </summary>
        public UnityEvent<object> Start;

        /// <summary>
        /// 종료 시 호출
        /// </summary>
        public UnityEvent Stop;
    }
}