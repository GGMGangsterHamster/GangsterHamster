using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sound;
using Characters.Player;

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

        public Action<float> soundAction { get; set; }
        public Action<float> sensitivityAction { get; set; }

        // UIAction�� ��ӹް� �ִ� Ŭ�������� _uiDict�� ��Ƽ� �����Ѵ�
        private Dictionary<UIPanels, UIAction> _uiDict = new Dictionary<UIPanels, UIAction>();

        private void Start()
        {
            soundAction += volume => {
                SoundManager.Instance.SetSound(volume);
                BackgroundMusic.Instance.SetVolume(volume);
            };

            sensitivityAction += sensitivity => {
                PlayerValues.MouseSpeed = sensitivity * 2 + 0.1f;
                //Debug.Log("Set sensitivity to " + PlayerValues.MouseSpeed);
            };

            for (int i = 0; i < _uiPanelParent.childCount; i++)
            {
                UIAction ui = _uiPanelParent.GetChild(i).GetComponent<UIAction>();
                UIPanels panelEnum = (UIPanels)ui.panelId;
                
                _uiDict.Add(panelEnum, ui);
                _uiDict[panelEnum].InitActions();
            }
        }

        private void Update()
        {
            // panelId�� ���� ū�ź��� for�� ������ Ȱ��ȭ �Ǿ�������
            // UpdateActions �Լ� ȣ����
            for(int i = _uiDict.Count; i >= 1; i--)
            {
                if(_uiDict[(UIPanels)i].gameObject.activeSelf)
                {
                    _uiDict[(UIPanels)i].UpdateActions();
                    break;
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
    }
}
