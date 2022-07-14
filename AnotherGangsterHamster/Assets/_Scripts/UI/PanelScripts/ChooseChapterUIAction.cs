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
        [SerializeField] private Button _disableButton;
        [SerializeField] private Transform _stageButtonParent;
        [SerializeField] private GameObject _stageButtonPrefab;

        void Awake()
        {
            _buttonClickSound = Resources.Load<AudioSource>("Audio/SoundEffect/10(ButtonUISound)");
        }

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
                _buttonClickSound.Play();
            });

            for(int i = (int)(StageNames.NONE + 1); i < (int)StageNames.END_OF_STAGE; i++)
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

            if (File.Exists("stageData" + ".txt"))
            {
                StreamReader sr = new StreamReader("stageData" + ".txt");
                string data = sr.ReadLine();

                StageNames stage = (StageNames)Enum.Parse(typeof(StageNames), sceneName);
                StageNames savedStage = (StageNames)Enum.Parse(typeof(StageNames), data);

                if(stage <= savedStage)
                {
                    stageButton.onClick.AddListener(() =>
                    {
                        _buttonClickSound.Play();
                        SceneManager.LoadScene(sceneName);
                        BackgroundMusic.Instance.StartBackgroundMusic();
                        Utils.LockCursor();
                        Utils.MoveTime();
                    });
                }
                else
                {
                    stageButton.image.color = new Color(1, 1, 1, 0.1f);
                }

                sr.Close();
            }

        }
    }

}