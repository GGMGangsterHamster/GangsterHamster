using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : MonoBehaviour, IUIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _fullScreenModeButton;
        [SerializeField] private Button _windowScreenModeButton;
        [SerializeField] private Button _1920x1080ResolutionButton;
        [SerializeField] private Button _2560x1080ResolutionButton;
        [SerializeField] private Button _disableButton;

        [SerializeField] private Scrollbar _soundScrollbar;
        [SerializeField] private Scrollbar _sensitivityScrollbar;

        public void ActivationActions()
        {
            // _soundScrollbar.value �� ���� ���� ������ ���� �ʱ�ȭ �����ְ� ��ȯ�Ǵ� ���� ���� �����ֱ⵵ �ؾ� ��
            // _sensitivityScrollbar.value �� ���� �ΰ��� ������ ���� �ʱ�ȭ �����ְ� ��ȯ�Ǵ� ���� ���� �����ֱ⵵ �ؾ� ��
        }

        public void DeActivationActions()
        {

        }

        public void InitActions()
        {
            _fullScreenModeButton.onClick.AddListener(() =>
            {
                // ��üȭ������ ��ȯ
            });

            _windowScreenModeButton.onClick.AddListener(() =>
            {
                // âȭ������ ��ȯ
            });

            _1920x1080ResolutionButton.onClick.AddListener(() =>
            {
                // 1920x1080 �ػ󵵷� ��ȯ
            });

            _2560x1080ResolutionButton.onClick.AddListener(() =>
            {
                // 2560x1080 �ػ󵵷� ��ȯ
            });

            _disableButton.onClick.AddListener(() =>
            {
                // ������ �г� ��Ȱ��ȭ
            });
        }
    }

}