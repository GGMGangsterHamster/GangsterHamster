using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// ��� �ϴ� �ൿ�� ���� �̺�Ʈ�� ȣ�����ִ� ���Ŀ
    /// </summary>

    public class LumoMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;
    }
}
