using System;

namespace Objects.Trigger
{
    /// <summary>
    /// 트리거 관리 용
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// 구별용
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 작동 여부
        /// </summary>
        public bool Activated { get; set; }

        /// <summary>
        /// 트리거 작동 시 호출<br/>
        /// OnTrigger(작동시킨 오브젝트);
        /// </summary>
        public Action<UnityEngine.GameObject> OnTrigger { get; set; }
    }
}