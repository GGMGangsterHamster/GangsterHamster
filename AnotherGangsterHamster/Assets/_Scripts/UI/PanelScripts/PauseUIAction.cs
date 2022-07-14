using Characters.Player;
using Objects.Trigger;
using Setting.VO;
using Stages.Management;
using UI.Screen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class PauseUIAction : UIAction
    {
        private string _soundPath = "SettingValue/Sound.json";
        private string _sensitivityPath = "SettingValue/Sensitivity.json";

        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _goTitleButton;
        [SerializeField] private Button _gameRestartButton;
        [SerializeField] private Button _disableButton;
        [SerializeField] private Button _stageSkipButton;
        [SerializeField] private Button _notDeadButton;
        [SerializeField] private Button _spectatorButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        void Awake()
        {
            _buttonClickSound = Resources.Load<AudioSource>("Audio/SoundEffect/6(ButtonEffectSound)");
        }

        public override void ActivationActions()
        {
            // ���⼭ ��ũ�ѹٵ��� ���� �ʱ�ȭ ������
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
            panelId = 6;

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

            _goTitleButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(StageNames.Title.ToString());
                _buttonClickSound.Play();
                Utils.MoveTime();
                // ���� UI ��Ȱ��ȭ ��Ű�� "Title UI"�� ��ȯ
            });

            _gameRestartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(gameObject.scene.name);
                _buttonClickSound.Play();
                Utils.LockCursor();
                Utils.MoveTime();
                // ������ ���������� �����
            });

            _disableButton.onClick.AddListener(() =>
            {
                _buttonClickSound.Play();
                Utils.LockCursor();
                Utils.MoveTime();
                UIManager.Instance.DeActivationPanel(panelId);
            });

            #region �ӽ�

            _stageSkipButton.onClick.AddListener(() =>
            {
                SceneLoadTrigger sceneLoadTrigger = GameObject.FindObjectOfType<SceneLoadTrigger>();
                SceneManager.LoadScene((sceneLoadTrigger.LoadTarget));
                Utils.LockCursor();
                Utils.MoveTime();
            });

            _notDeadButton.onClick.AddListener(() =>
            {
                GameObject.FindObjectOfType<Player>().SetMaxHP(int.MaxValue);
            });

            _spectatorButton.onClick.AddListener(() =>
            {
                Spectator.Instance.StartSpectorMode();
            });

            #endregion


            SoundVO soundVO = Utils.JsonToVO<SoundVO>(_soundPath);
            SensitivityVO sensitivityVO = Utils.JsonToVO<SensitivityVO>(_sensitivityPath);

            UIManager.Instance.soundAction(soundVO.master);

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
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Utils.LockCursor();
                Utils.MoveTime();
                UIManager.Instance.DeActivationPanel(panelId);
            }
        }
    }

}