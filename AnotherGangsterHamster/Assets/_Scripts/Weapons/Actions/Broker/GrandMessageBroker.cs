using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// �׷��尡 �ϴ� �ൿ�� ���� �̺�Ʈ�� ȣ�����ִ� ���Ŀ
    /// </summary>
    
    public class GrandMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;
    }
}
