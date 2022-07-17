using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// 모든 무기가 하는 행동에 따라 이벤트를 호출해주는 브로커
    /// </summary>

    public class AllWeaponMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;
        public UnityEvent OnChangeWeapon;
    }
}
