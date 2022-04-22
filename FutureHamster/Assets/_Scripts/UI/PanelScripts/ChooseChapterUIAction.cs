using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class ChooseChapterUIAction : UIAction
    {
        [Header("������ ����� �ִ� UI��")]
        [SerializeField] private Button _disableButton;
        [SerializeField] private Transform _stageButtonParent;

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 3;

            _disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
            });

            for(int i = 0; i < _stageButtonParent.childCount; i++)
            {
                Button stageButton = _stageButtonParent.GetChild(i).GetComponent<Button>();

                stageButton.onClick.AddListener(() =>
                {
                    // ��ư�� ���� �� �������� �ε� (���� Ŭ���� ���� ���� ���������� �� ó��)
                });
            }
        }

        public override void UpdateActions()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.DeActivationPanel(panelId);
            }
        }
    }

}