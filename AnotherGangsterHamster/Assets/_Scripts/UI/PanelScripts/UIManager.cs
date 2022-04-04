using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.PanelScripts
{
    public enum UIPanels
    {
        Title,
        NewGame,
        ChooseChapter,
        Option,
        InGame,
        Pause,
    }

    public class UIManager : MonoSingleton<UIManager>
    {
        // UIAction�� ��ӹް� �ִ� Ŭ�������� _uiDict�� ��Ƽ� �����Ѵ�
        private Dictionary<UIPanels, IUIAction> _uiDict = new Dictionary<UIPanels, IUIAction>();

        private void Start()
        {
            IUIAction[] _uiActions = FindObjectsOfType<UIAction>();

            foreach (UIAction uiAction in _uiActions)
            {
                uiAction.InitActions();
                uiAction.gameObject.SetActive(false);

                _uiDict[(UIPanels)uiAction.panelId] = uiAction;
            }
        }

        // �ش��ϴ� �г� Ȱ��ȭ
        public void ActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].ActivationActions();

            ((UIAction)_uiDict[panelEnum]).gameObject.SetActive(true);
        }

        // �ش��ϴ� �г� ��Ȱ��ȭ
        public void DeActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].DeActivationActions();

            ((UIAction)_uiDict[panelEnum]).gameObject.SetActive(false);
        }

        // UI �г� ��Ƽ� �������̽��� �Լ� ȣ�� ���ֱ�
        // �� �̿ܿ��� �˾Ƽ� ���ϱ�
    }
}
