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
        private Dictionary<UIPanels, UIAction> _uiDict = new Dictionary<UIPanels, UIAction>();

        private void Start()
        {
            for(int i = 0; i < _uiPanelParent.childCount; i++)
            {
                UIAction ui = _uiPanelParent.GetChild(i).GetComponent<UIAction>();
                UIPanels panelEnum = (UIPanels)ui.panelId;
                
                _uiDict[panelEnum] = ui;
                _uiDict[panelEnum].InitActions();
            }

            foreach(UIAction ac in _uiDict.Values)
            {
                Debug.Log(ac.name);
            }
        }

        private void Update()
        {
            for(int i = _uiDict.Count; i >= 1; i--)
            {
                if(_uiDict[(UIPanels)i].gameObject.activeSelf)
                {
                    _uiDict[(UIPanels)i].UpdateActions();
                }
            }
        }

        // �ش��ϴ� �г� Ȱ��ȭ
        public void ActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].ActivationActions();

            _uiDict[panelEnum].gameObject.SetActive(true);
        }

        public void ActivationPanel(int panelId)
        {
            _uiDict[(UIPanels)panelId].ActivationActions();

            _uiDict[(UIPanels)panelId].gameObject.SetActive(true);
        }

        // �ش��ϴ� �г� ��Ȱ��ȭ
        public void DeActivationPanel(UIPanels panelEnum)
        {
            _uiDict[panelEnum].DeActivationActions();

            _uiDict[panelEnum].gameObject.SetActive(false);
        }

        public void DeActivationPanel(int panelId)
        {
            _uiDict[(UIPanels)panelId].DeActivationActions();

            _uiDict[(UIPanels)panelId].gameObject.SetActive(false);
        }

        // UI �г� ��Ƽ� �������̽��� �Լ� ȣ�� ���ֱ�
        // �� �̿ܿ��� �˾Ƽ� ���ϱ�
    }
}
