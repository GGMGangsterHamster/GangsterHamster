using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class NewGameUIAction : MonoBehaviour, IUIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _disableButton;
        [SerializeField] private Button _acceptButton;

        public void ActivationActions()
        {

        }

        public void DeActivationActions()
        {

        }

        public void InitActions()
        {
            _disableButton.onClick.AddListener(() =>
            {
                // ���� Ȱ��ȭ �Ǿ� �ִ� �г� ��Ȱ��ȭ
            });

            _acceptButton.onClick.AddListener(() =>
            {
                // ������ ���� ������ ��� ����, ������ ó������ ����� �� "In Game UI"�� Ȱ��ȭ
            });
        }
    }
}
