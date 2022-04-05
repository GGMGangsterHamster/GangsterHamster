using Setting.VO;
using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : UIAction
    {
        public string soundPath = "SettingValue/Sound";
        public string sensitivityPath = "SettingValue/Sensitivity";

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
            // ���⼭ ��ũ�ѹٵ��� ���� �ʱ�ȭ ������
            SoundVO soundVO = Utils.JsonToVO<SoundVO>(soundPath);
            SensitivityVO sensitivityVO = Utils.JsonToVO<SensitivityVO>(sensitivityPath);

            _soundScrollbar.value = soundVO.master;
            _sensitivityScrollbar.value = sensitivityVO.sensitivity;
        }

        public override void DeActivationActions()
        {

            SoundVO soundVO = new SoundVO(_soundScrollbar.value);
            SensitivityVO sensitivityVO = new SensitivityVO(_sensitivityScrollbar.value);

            Utils.VOToJson(soundPath, soundVO);
            Utils.VOToJson(sensitivityPath, sensitivityVO);
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

            _soundScrollbar.onValueChanged.AddListener(value =>
            {
                // �ٲ�� ������ ��򰡿� �����ϰ�
                // �� ������ �ٸ� �������� �����Ѵ�
            });

            _sensitivityScrollbar.onValueChanged.AddListener(value =>
            {
                // �ٲ�� ������ ��򰡿� �����ϰ�
                // �� ������ �ٸ� �������� �����Ѵ�
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