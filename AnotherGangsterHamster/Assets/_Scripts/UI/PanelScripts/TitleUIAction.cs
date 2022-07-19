using Stages.Management;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace UI.PanelScripts
{
    public class TitleUIAction : UIAction
    {
        [Header("������ ����� �ִ� UI��")]
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

            // ���� ���̺�� ���൵�� ���ٸ� ��ư ���� ��Ӱ� �ϱ�

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
            // Ÿ��Ʋ���� üũ �� �� ���� 
            // ���� �߰� �Ҽ���
        }
    }

}