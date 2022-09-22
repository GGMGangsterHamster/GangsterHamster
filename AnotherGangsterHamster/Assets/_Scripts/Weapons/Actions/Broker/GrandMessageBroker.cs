using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// 그랜드가 하는 행동에 따라 이벤트를 호출해주는 브로커
    /// </summary>
    
    public class GrandMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent<int> OnUse; // 0, 1, 2를 인자값으로 주는데 각각 1m, 2m, 4m 단계로 변한 것을 의미함
        public UnityEvent OnReset;
        public UnityEvent OnRebound;
        public UnityEvent OnChangedMinSize;
        public UnityEvent OnChangedOneStep;
        public UnityEvent OnChangedTwoStep;
        public UnityEvent OnChangedEnd;
    }
}
