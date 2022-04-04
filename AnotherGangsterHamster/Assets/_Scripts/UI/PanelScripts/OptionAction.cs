using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : MonoBehaviour, IUIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _disableButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        public void ActivationActions()
        {
            // _soundScrollbar.value 를 지금 사운드 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함
            // _sensitivityScrollbar.value 를 지금 민감도 설정에 따라서 초기화 시켜주고 변환되는 값을 적용 시켜주기도 해야 함
        }

        public void DeActivationActions()
        {

        }

        public void InitActions()
        {
            _fullScreenModeButton.onClick.AddListener(() =>
            {
                // 전체화면으로 변환
            });

            _windowScreenModeButton.onClick.AddListener(() =>
            {
                // 창화면으로 전환
            });

            _1920x1080ResolutionButton.onClick.AddListener(() =>
            {
                // 1920x1080 해상도로 전환
            });

            _2560x1080ResolutionButton.onClick.AddListener(() =>
            {
                // 2560x1080 해상도로 전환
            });

            _disableButton.onClick.AddListener(() =>
            {
                // 현재의 패널 비활성화
            });
        }
    }

}