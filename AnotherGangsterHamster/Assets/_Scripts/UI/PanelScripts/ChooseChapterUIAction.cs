using Stages.Management;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class ChooseChapterUIAction : UIAction
    {
        [Header("각자의 기능이 있는 UI들")]
        public Button disableButton;
        [SerializeField] private Transform _stageButtonParent;

        private Dictionary<int, Button> stageButtonDic = new Dictionary<int, Button>();

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 3;

            disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
            });

            // Scrollbar 안에 있는 버튼 다 Dictionary에 넣기
            for(int i = 0; i < _stageButtonParent.childCount; i++)
            {
                stageButtonDic.Add(i - 1, _stageButtonParent.GetChild(i).GetComponent<Button>());
            }

            for (int i = (int)(StageNames.NONE + 1); i < (int)StageNames.Ending; i++)
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
            if (File.Exists("stageData" + ".txt"))
            {
                StreamReader sr = new StreamReader("stageData" + ".txt");
                string data = sr.ReadLine();

                StageNames stage = (StageNames)Enum.Parse(typeof(StageNames), sceneName);
                StageNames savedStage;

                if (data == null)
                {
                    savedStage = StageNames.NONE;
                }
                else
                {
                    savedStage = (StageNames)Enum.Parse(typeof(StageNames), data);
                }

                if(stage <= savedStage)
                {
                    stageButtonDic[(int)stage].onClick.AddListener(() =>
                    {
                        SceneManager.LoadScene(sceneName);
                        BackgroundMusic.Instance.StartBackgroundMusic();
                        Utils.LockCursor();
                        Utils.MoveTime();
                    });
                }
                else
                {
                    stageButtonDic[(int)stage].image.color = new Color(1, 1, 1, 0.1f);
                }

                sr.Close();
            }

        }
    }

}