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

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 1;

            _continueButton.onClick.AddListener(() =>
            {
                // ���� ù ������ �ƴ϶�� => �� ���� ó���� �ٸ��� ������� �ٵ� �� �ٸ��� ����
                // ����Ǿ� �ִ� �����͸� �ҷ����� "In Game UI"�� Ȱ��ȭ ��Ų��
            });

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
    }

}