using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// 그라비토가 하는 행동에 따라 이벤트를 호출해주는 브로커
    /// </summary>

    public class GravitoMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;

        // 플레이어 회전 락 때문에 추가함 -우앱
        public UnityEvent OnLerpEnd;
    }
}
