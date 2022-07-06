using Stages.Management;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace UI.PanelScripts
{
    public class TitleUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _chooseChapterButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _exitButton;

        string _fullpath = "stageData";

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 1;

            // 만약 세이브된 진행도가 없다면 버튼 색을 어둡게 하기

            if (!StageManager.Instance.ExistsStage())
            {
                _continueButton.image.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                _continueButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.LoadStage();
                });
            }

            _newGameButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.NewGame);
            });

            _chooseChapterButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.ChooseChapter);
            });

            _optionButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.Option);
            });

            _exitButton.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }

        public override void UpdateActions()
        {
            // 타이틀에선 체크 할 것 없음 
            // 추후 추가 할수도
        }
    }

}