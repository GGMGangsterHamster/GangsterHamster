using UnityEngine;
using UnityEngine.Events;

namespace Weapons.Actions.Broker
{
    /// <summary>
    /// �׶���䰡 �ϴ� �ൿ�� ���� �̺�Ʈ�� ȣ�����ִ� ���Ŀ
    /// </summary>

    public class GravitoMessageBroker : MonoBehaviour
    {
        public UnityEvent OnFire;
        public UnityEvent OnUse;
        public UnityEvent OnReset;

        // �÷��̾� ȸ�� �� ������ �߰��� -���
        public UnityEvent OnLerpEnd;
    }
}
