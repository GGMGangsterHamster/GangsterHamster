using Setting.VO;
using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : UIAction
    {
        private string _soundPath = "SettingValue/Sound.json";
        private string _sensitivityPath = "SettingValue/Sensitivity.json";

        [Header("각자의 기능이 있는 UI들")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _disableButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        void Awake()
        {
            _buttonClickSound = Resources.Load<AudioSource>("Audio/SoundEffect/10(ButtonUISound)");
        }

        public override void ActivationActions()
        {
            // 여기서 스크롤바들의 값을 초기화 시켜줌
            SoundVO soundVO = Utils.JsonToVO<SoundVO>(_soundPath);
            SensitivityVO sensitivityVO = Utils.JsonToVO<SensitivityVO>(_sensitivityPath);

            _soundScrollbar.value = soundVO.master;
            _sensitivityScrollbar.value = sensitivityVO.sensitivity;
        }

        public override void DeActivationActions()
        {

            SoundVO soundVO = new SoundVO(_soundScrollbar.value);
            SensitivityVO sensitivityVO = new SensitivityVO(_sensitivityScrollbar.value);

            Utils.VOToJson(_soundPath, soundVO);
            Utils.VOToJson(_sensitivityPath, sensitivityVO);
        }

        public override void InitActions()
        {
            panelId = 4;

            _fullScreenModeButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetFullScreen();
                _buttonClickSound.Play();
            });

            _windowScreenModeButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetWindowScreen();
                _buttonClickSound.Play();
            });

            _1920x1080ResolutionButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetResolution(1920, 1080);
                _buttonClickSound.Play();
            });

            _2560x1080ResolutionButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetResolution(2560, 1080);
                _buttonClickSound.Play();
            });

            _disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
                _buttonClickSound.Play();
            });

            _soundScrollbar.onValueChanged.AddListener(value =>
            {
                UIManager.Instance.soundAction(value);
            });

            _sensitivityScrollbar.onValueChanged.AddListener(value =>
            {
                UIManager.Instance.sensitivityAction(value);
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