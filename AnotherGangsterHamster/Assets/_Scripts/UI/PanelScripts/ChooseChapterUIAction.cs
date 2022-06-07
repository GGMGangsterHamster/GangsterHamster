using Stages.Management;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class ChooseChapterUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        [SerializeField] private Button _disableButton;
        [SerializeField] private Transform _stageButtonParent;
        [SerializeField] private GameObject _stageButtonPrefab;

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

            for(int i = 1; i < (int)StageNames.END_OF_STAGE; i++)
            {
                SpawnChapter(((StageNames)i).ToString());
            }
        }

        public override void UpdateActions()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.DeActivationPanel(panelId);
            }
        }

        // 지금은 이름만 있따
        private void SpawnChapter(string sceneName)
        {
            GameObject obj = Instantiate(_stageButtonPrefab, _stageButtonParent);

            obj.transform.GetChild(0).GetComponent<Text>().text = sceneName;

            Button stageButton = obj.GetComponent<Button>();

            stageButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(sceneName);
                Utils.LockCursor();
                Utils.MoveTime();
            });
        }
    }

}