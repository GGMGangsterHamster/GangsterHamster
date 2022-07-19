using Stages.Management;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace UI.PanelScripts
{
    public class TitleUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        public Button continueButton;
        public Button newGameButton;
        public Button chooseChapterButton;
        public Button optionButton;
        public Button exitButton;

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
                continueButton.image.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                continueButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.LoadStage();
                });
            }

            newGameButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.NewGame);
            });

            chooseChapterButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.ChooseChapter);
            });

            optionButton.onClick.AddListener(() =>
            {
                UIManager.Instance.ActivationPanel(UIPanels.Option);
            });

            exitButton.onClick.AddListener(() =>
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