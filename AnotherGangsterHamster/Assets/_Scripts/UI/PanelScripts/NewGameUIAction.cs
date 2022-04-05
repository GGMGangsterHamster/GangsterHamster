using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class NewGameUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
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
                // 기존의 저장 데이터 모두 삭제, 게임을 처음부터 재시작 후 "In Game UI"를 활성화
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
