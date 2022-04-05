using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class ChooseChapterUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
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
                    // 버튼에 저장 된 스테이지 로딩 (아직 클리어 하지 못한 스테이지는 블러 처리)
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