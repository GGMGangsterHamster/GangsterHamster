using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// ��� ���Ⱑ �ϴ� �ൿ�� ���� �̺�Ʈ�� ȣ�����ִ� ���Ŀ
    /// </summary>

    public class AllWeaponMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;
        public UnityEvent OnChangeWeapon;
    }
}
