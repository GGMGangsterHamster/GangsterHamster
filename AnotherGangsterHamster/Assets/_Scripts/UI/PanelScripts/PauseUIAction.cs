using Setting.VO;
using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class PauseUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _goTitleButton;
        [SerializeField] private Button _gameRestartButton;
        [SerializeField] private Button _disableButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        public override void ActivationActions()
        {
            // _soundScrollbar.value 를 지금 사운드 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함
            // _sensitivityScrollbar.value 를 지금 민감도 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 6;

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

            _goTitleButton.onClick.AddListener(() =>
            {
                // 현재 UI 비활성화 시키고 "Title UI"로 변환
            });

            _gameRestartButton.onClick.AddListener(() =>
            {
                // 현재의 스테이지를 재시작
            });

            _disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
            });

            _soundScrollbar.onValueChanged.AddListener(value =>
            {

            });

            _sensitivityScrollbar.onValueChanged.AddListener(value =>
            {

            });
        }

        public override void UpdateActions()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.DeActivationPanel(panelId);
            }
        }
    }

}