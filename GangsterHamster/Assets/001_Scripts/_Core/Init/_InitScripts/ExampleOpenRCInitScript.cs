using System;
using OpenRC;
using UnityEngine;


namespace Objects.Init
{

    public class ExampleOpenRCInitScript : IInitBase
    {
        /// <summary>
        /// 의존성 해결<br/>
        /// 지정된 시작 호출 전 호출됨
        /// </summary>
        public void Depend(MonoBehaviour mono)
        {

        }

        /// <summary>
        /// 지정된 RunLevel 에 호출
        /// </summary>
        public void Start(object param)
        {
            
        }

        /// <summary>
        /// 종료 시 호출
        /// </summary>
        public void Stop()
        {
            
        }
    }
}