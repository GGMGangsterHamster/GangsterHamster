using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class NewGameUIAction : UIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _disableButton;
        [SerializeField] private Button _acceptButton;

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 2;

            _disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
            });

            _acceptButton.onClick.AddListener(() =>
            {
                // ������ ���� ������ ��� ����, ������ ó������ ����� �� "In Game UI"�� Ȱ��ȭ
            });
        }

        public override void UpdateActions()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.DeActivationPanel(panelId);
            }
        }
    }
}
