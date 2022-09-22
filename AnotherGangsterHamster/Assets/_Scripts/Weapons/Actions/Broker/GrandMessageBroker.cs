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
        public UnityEvent<int> OnUse; // 0, 1, 2�� ���ڰ����� �ִµ� ���� 1m, 2m, 4m �ܰ�� ���� ���� �ǹ���
        public UnityEvent OnReset;
        public UnityEvent OnRebound;
        public UnityEvent OnChangedMinSize;
        public UnityEvent OnChangedOneStep;
        public UnityEvent OnChangedTwoStep;
        public UnityEvent OnChangedEnd;
    }
}
