using Setting.VO;
using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : UIAction
    {
        public string resourcesSoundPath;
        public string resourcesSensitivityPath;

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
            //_soundScrollbar.value = Utils.JsonToVO<SoundVO>(resourcesSoundPath).master;
            //_sensitivityScrollbar.value = Utils.JsonToVO<SensitivityVO>(resourcesSensitivityPath).sensitivity;
        }

        public override void DeActivationActions()
        {
//            SoundVO soundVo = new SoundVO(_soundScrollbar.value);
//            SensitivityVO senVo = new SensitivityVO(_sensitivityScrollbar.value);

//            Debug.Log(soundVo.master);
//            Debug.Log(senVo.sensitivity);
//#if UNITY_EDITOR
//            Utils.VOToJson(Application.dataPath + "/Resources/" + resourcesSoundPath + ".json", soundVo);
//            Utils.VOToJson(Application.dataPath + "/Resources/" + resourcesSensitivityPath + ".json", senVo);
//#else
//            //Utils.VOToJson(Application.persistentDataPath + "/Resources/" + resourcesSoundPath + ".json", soundVo);
//            //Utils.VOToJson(Application.persistentDataPath + "/Resources/" + resourcesSensitivityPath + ".json", senVo);
//#endif
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