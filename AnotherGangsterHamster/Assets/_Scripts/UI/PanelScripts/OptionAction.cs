using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : UIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _disableButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        public override void ActivationActions()
        {
            // _soundScrollbar.value �� ���� ���� ������ ���� �ʱ�ȭ �����ְ� ��ȯ�Ǵ� ���� ���� �����ֱ⵵ �ؾ� ��
            // _sensitivityScrollbar.value �� ���� �ΰ��� ������ ���� �ʱ�ȭ �����ְ� ��ȯ�Ǵ� ���� ���� �����ֱ⵵ �ؾ� ��
        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 4;

            _fullScreenModeButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetFullScreen();
            });

            _windowScreenModeButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetWindowScreen();
            });

            _1920x1080ResolutionButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetResolution(1920, 1080);
            });

            _2560x1080ResolutionButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetResolution(2560, 1080);
            });

            _disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
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