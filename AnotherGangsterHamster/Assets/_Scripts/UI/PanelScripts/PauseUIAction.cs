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
        public Button fullScreenModeButton;
        public Button windowScreenModeButton;
        public Button _1920x1080ResolutionButton;
        public Button _2560x1080ResolutionButton;
        public Button goTitleButton;
        public Button gameRestartButton;
        public Button disableButton;
        public Button stageSkipButton;
        public Button notDeadButton;
        public Button spectatorButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

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

            fullScreenModeButton.onClick.AddListener(() =>
            {
                ScreenManager.Instance.SetFullScreen();
            });

            windowScreenModeButton.onClick.AddListener(() =>
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

            goTitleButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(StageNames.Title.ToString());
                Utils.MoveTime();
                // ���� UI ��Ȱ��ȭ ��Ű�� "Title UI"�� ��ȯ
            });

            gameRestartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(gameObject.scene.name);
                Utils.LockCursor();
                Utils.MoveTime();
                // ������ ���������� �����
            });

            disableButton.onClick.AddListener(() =>
            {
                Utils.LockCursor();
                Utils.MoveTime();
                UIManager.Instance.DeActivationPanel(panelId);
            });

            #region �ӽ�

            stageSkipButton.onClick.AddListener(() =>
            {
                SceneLoadTrigger sceneLoadTrigger = GameObject.FindObjectOfType<SceneLoadTrigger>();
                SceneManager.LoadScene((sceneLoadTrigger.LoadTarget));
                Utils.LockCursor();
                Utils.MoveTime();
            });

            notDeadButton.onClick.AddListener(() =>
            {
                GameObject.FindObjectOfType<Player>().SetMaxHP(int.MaxValue);
            });

            spectatorButton.onClick.AddListener(() =>
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