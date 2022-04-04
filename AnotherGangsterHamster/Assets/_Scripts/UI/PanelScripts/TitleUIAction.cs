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
                // ���� ù ������ �ƴ϶��
                // ����Ǿ� �ִ� �����͸� �ҷ����� "In Game UI"�� Ȱ��ȭ ��Ų��
            });

            _newGameButton.onClick.AddListener(() =>
            {
                // "New Game UI"�� Ȱ��ȭ ��Ų��
            });

            _chooseChapterButton.onClick.AddListener(() =>
            {
                // "Choose Chapter UI"�� Ȱ��ȭ ��Ų��
            });

            _optionButton.onClick.AddListener(() =>
            {
                // "Option UI"�� Ȱ��ȭ ��Ų��
            });

            _exitButton.onClick.AddListener(() =>
            {
                // ������ �����Ų��
            });
        }
    }

}