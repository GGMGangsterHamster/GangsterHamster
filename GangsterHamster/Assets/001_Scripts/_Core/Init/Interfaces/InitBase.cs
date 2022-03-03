using System;
using System.Collections.Generic;

namespace OpenRC
{
    [Serializable]
    abstract public class InitBase : UnityEngine.Component
    {
        abstract public string Name { get; }
        abstract public RunLevel RunLevel { get; }

        /// <summary>
        /// 의존성 해결<br/>
        /// 지정된 시작 호출 전 호출됨
        /// </summary>
        abstract public void Depend(UnityEngine.MonoBehaviour mono);

        /// <summary>
        /// 지정된 RunLevel 에 호출
        /// </summary>
        abstract public void Call(object param);

        /// <summary>
        /// 종료 시 호출
        /// </summary>
        abstract public void Stop();
    }
}