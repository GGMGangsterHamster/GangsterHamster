using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.PanelScripts
{
    public enum UIPanels
    {
        Title = 1,
        NewGame,
        ChooseChapter,
        Option,
        InGame,
        Pause,
    }

    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private Transform _uiPanelParent;

        // UIAction�� ��ӹް� �ִ� Ŭ�������� _uiDict�� ��Ƽ� �����Ѵ�
        private Dictionary<UIPanels, IUIAction> _uiDict = new Dictionary<UIPanels, IUIAction>();

        private void Start()
        {
            for(int i = 0; i < _uiPanelParent.childCount; i++)
            {
                IUIAction iui = _uiPanelParent.GetChild(i).GetComponent<IUIAction>();
                UIPanels panelEnum = (UIPanels)(((UIAction)iui).panelId);
                
                _uiDict[panelEnum] = iui;
                _uiDict[panelEnum].InitActions();
            }

            foreach(UIAction ac in _uiDict.Values)
            {
                Debug.Log(ac.name);
            }
        }

        // �ش��ϴ� �г� Ȱ��ȭ
        public void ActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].ActivationActions();

            ((UIAction)_uiDict[panelEnum]).gameObject.SetActive(true);
        }

        public void ActivationPanel(int panelId)
        {
            _uiDict[(UIPanels)panelId].ActivationActions();

            ((UIAction)_uiDict[(UIPanels)panelId]).gameObject.SetActive(true);
        }

        // �ش��ϴ� �г� ��Ȱ��ȭ
        public void DeActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].DeActivationActions();

            ((UIAction)_uiDict[panelEnum]).gameObject.SetActive(false);
        }

        public void DeActivationPanel(int panelId)
        {
            _uiDict[(UIPanels)panelId].DeActivationActions();

            ((UIAction)_uiDict[(UIPanels)panelId]).gameObject.SetActive(false);
        }

        // UI �г� ��Ƽ� �������̽��� �Լ� ȣ�� ���ֱ�
        // �� �̿ܿ��� �˾Ƽ� ���ϱ�
    }
}
