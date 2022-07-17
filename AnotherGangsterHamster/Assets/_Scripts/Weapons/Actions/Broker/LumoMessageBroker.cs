using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// 루모가 하는 행동에 따라 이벤트를 호출해주는 브로커
    /// </summary>

    public class LumoMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;
    }
}
