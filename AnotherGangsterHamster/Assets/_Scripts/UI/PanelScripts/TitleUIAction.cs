using Stages.Management;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace UI.PanelScripts
{
    public class TitleUIAction : UIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _chooseChapterButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _exitButton;

        AudioSource _buttonClickSound;
        string _fullpath = "stageData";

        void Awake()
        {
            _buttonClickSound = Resources.Load<AudioSource>("Audio/SoundEffect/6(ButtonEffectSound)");
        }

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 1;

            // ���� ���̺�� ���൵�� ���ٸ� ��ư ���� ��Ӱ� �ϱ�

            if (!StageManager.Instance.ExistsStage())
            {
                _continueButton.image.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                _continueButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.LoadStage();
                    _buttonClickSound.Play();
                });
            }

            _newGameButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.NewGame);
                _buttonClickSound.Play();
            });

            _chooseChapterButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.ChooseChapter);
                _buttonClickSound.Play();
            });

            _optionButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.Option);
                _buttonClickSound.Play();

            });

            _exitButton.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                _buttonClickSound.Play();
#else
                Application.Quit();
#endif
            });
        }

        public override void UpdateActions()
        {
            // Ÿ��Ʋ���� üũ �� �� ���� 
            // ���� �߰� �Ҽ���
        }
    }

}