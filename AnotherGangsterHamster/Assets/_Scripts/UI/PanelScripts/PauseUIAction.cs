using Characters.Player;
using Objects.Trigger;
using Setting.VO;
using Sound;
using Stages.Management;
using UI.Screen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

namespace UI.PanelScripts
{
    enum ScreenMode
    {
        FullScreen,
        WindowScreen,
    }
    public class PauseUIAction : UIAction
    {
        #region SavePaths
        private string _soundPath = "SettingValue/Sound.json";
        private string _mousePath = "SettingValue/Sensitivity.json";
        private string _screenPath = "SettingValue/Screen.json";

        private string _graphicPath = "SettingValue/Graphic.json";
        #endregion
          
        [Header("Sound")]
        public Scrollbar soundScrollbar;

        [Header("Mouse")]
        public Scrollbar mouseScrollbar;

        [Header("Screen")]
        public Dropdown screenModeDropdown;
        public Dropdown resolutionDropdown;

        [Header("Graphic")]
        public Dropdown graphicQualityDropdown;
        public Dropdown shadowDropdown;
        public Scrollbar gammaScrollbar;
        public Dropdown bloomDropdown;
        public Dropdown lightingDropdown;
        public Dropdown motionBlurDropdown;
        public Dropdown antialiasingDropdown;

        [Header("AntiAliasing")]
        public Dropdown taaQualityDropdown;
        public Scrollbar taaSharpenScrollbar;
        public Dropdown smaaQualityDropdown;

        [Header("Buttons")]
        public Button goTitleButton;
        public Button gameRestartButton;
        public Button disableButton;
        public Button resetButton;

        [Header("HDRP Settings")]
        public VolumeProfile globalVolume;

        private Tonemapping tonemapping;
        private Bloom bloom;
        private ScreenSpaceReflection ssr;
        private GlobalIllumination globalillumination;
        private AmbientOcclusion ambientOcclusion;
        private MotionBlur motionBlur;
        private HDAdditionalCameraData hdCameraData;

        public override void ActivationActions()
        {
            // 여기서 스크롤바들의 값을 초기화 시켜줌
            SoundVO soundVO = Utils.JsonToVO<SoundVO>(_soundPath);
            MouseVO mouseVO = Utils.JsonToVO<MouseVO>(_mousePath);
            ScreenVO screenVO = Utils.JsonToVO<ScreenVO>(_screenPath);
            GraphicVO graphicVO = Utils.JsonToVO<GraphicVO>(_graphicPath);

            if(soundVO != null)
                soundScrollbar.value = soundVO.master;
            if(mouseVO != null)
                mouseScrollbar.value = mouseVO.sensitivity;

            if(screenVO != null)
            {
                screenModeDropdown.value = screenVO.isFullScreen ? 0 : 1;
                resolutionDropdown.value = GetResolutionIndex(screenVO.width, screenVO.height);
            }

            if(graphicVO != null)
            {
                tonemapping.gamma.value = graphicVO.gamma + 0.2f;
                bloom.quality.value = graphicVO.bloom;
                ssr.quality.value = graphicVO.lighting;
                globalillumination.quality.value = graphicVO.lighting;
                ambientOcclusion.quality.value = graphicVO.shadow;
                motionBlur.quality.value = graphicVO.motionBlur;

                hdCameraData.renderingPathCustomFrameSettings.lodBiasQualityLevel = graphicVO.graphicQuality;
                hdCameraData.renderingPathCustomFrameSettings.maximumLODLevelQualityLevel = graphicVO.graphicQuality;
                hdCameraData.renderingPathCustomFrameSettings.sssQualityLevel = graphicVO.graphicQuality;
                hdCameraData.antialiasing = (HDAdditionalCameraData.AntialiasingMode)graphicVO.antialiasing;
                hdCameraData.TAAQuality = (HDAdditionalCameraData.TAAQualityLevel)graphicVO.taaQuality;
                hdCameraData.taaSharpenStrength = graphicVO.taaSharpen * 2;
                hdCameraData.SMAAQuality = (HDAdditionalCameraData.SMAAQualityLevel)graphicVO.smaaQuality;

                ActiveAntialiasingOption((HDAdditionalCameraData.AntialiasingMode)graphicVO.antialiasing);

                graphicQualityDropdown.value = graphicVO.graphicQuality;
                shadowDropdown.value = graphicVO.shadow;
                gammaScrollbar.value = graphicVO.gamma;
                bloomDropdown.value = graphicVO.bloom;
                lightingDropdown.value = graphicVO.lighting;
                motionBlurDropdown.value = graphicVO.motionBlur;
                antialiasingDropdown.value = graphicVO.antialiasing;
                taaQualityDropdown.value = graphicVO.taaQuality;
                taaSharpenScrollbar.value = graphicVO.taaSharpen;
                smaaQualityDropdown.value = graphicVO.smaaQuality;
            }
            else
            {
                ResetSetting();
            }

            SoundManager.Instance.MuteSound(true);
        }

        public override void DeActivationActions()
        {
            SoundVO soundVO = new SoundVO(soundScrollbar.value);
            MouseVO mouseVO = new MouseVO(mouseScrollbar.value);
            GraphicVO graphicVO = new GraphicVO(graphicQualityDropdown.value, shadowDropdown.value, gammaScrollbar.value, bloomDropdown.value, lightingDropdown.value, motionBlurDropdown.value, antialiasingDropdown.value,
                                                taaQualityDropdown.value, taaSharpenScrollbar.value, smaaQualityDropdown.value);

            Utils.VOToJson(_soundPath, soundVO);
            Utils.VOToJson(_mousePath, mouseVO);
            Utils.VOToJson(_graphicPath, graphicVO);

            SoundManager.Instance.MuteSound(false);
        }

        public override void InitActions()
        {
            panelId = 6;

            globalVolume.TryGet(out tonemapping);
            globalVolume.TryGet(out bloom);
            globalVolume.TryGet(out ssr);
            globalVolume.TryGet(out globalillumination);
            globalVolume.TryGet(out ambientOcclusion);
            globalVolume.TryGet(out motionBlur);

            hdCameraData = Camera.main.GetComponent<HDAdditionalCameraData>();


            SoundVO soundVO = Utils.JsonToVO<SoundVO>(_soundPath);
            MouseVO mouseVO = Utils.JsonToVO<MouseVO>(_mousePath);


            if (soundVO != null)
            {
                UIManager.Instance.soundAction(soundVO.master);
                Debug.Log($"사운드 값 세팅 : {soundVO.master}");
            }
            else
            {
                UIManager.Instance.soundAction(0.8f);
                Debug.Log("사운드 값 파일이 존재하지 않아 세팅이 되지 않음 기본값 : 0.8");
            }
            if (mouseVO != null)
            {
                UIManager.Instance.sensitivityAction(mouseVO.sensitivity);
                Debug.Log($"마우스 민감도 세팅 : {mouseVO.sensitivity}");
            }
            else
            {
                UIManager.Instance.sensitivityAction(0.8f);
                Debug.Log("마우스 민감도 값 파일이 존재하지 않아 세팅이 되지 않음 기본값 : 0.8");
            }


            screenModeDropdown.onValueChanged.AddListener(value =>
            {
                SetScreenMode((ScreenMode)value);
            });

            resolutionDropdown.onValueChanged.AddListener(value =>
            {
                SetResolution(resolutionDropdown.options[value].text);
            });

            gammaScrollbar.onValueChanged.AddListener(value =>
            {
                tonemapping.gamma.value = value + 0.2f;
            });

            bloomDropdown.onValueChanged.AddListener(value =>
            {
                bloom.quality.value = value;
            });

            lightingDropdown.onValueChanged.AddListener(value =>
            {
                ssr.quality.value = value;
                globalillumination.quality.value = value;
            });

            shadowDropdown.onValueChanged.AddListener(value =>
            {
                ambientOcclusion.quality.value = value;
            });

            motionBlurDropdown.onValueChanged.AddListener(value =>
            {
                motionBlur.active = value == 0;
            });

            graphicQualityDropdown.onValueChanged.AddListener(value =>
            {
                hdCameraData.renderingPathCustomFrameSettings.lodBiasQualityLevel = value;
                hdCameraData.renderingPathCustomFrameSettings.maximumLODLevelQualityLevel = value;
                hdCameraData.renderingPathCustomFrameSettings.sssQualityLevel = value;
            });

            antialiasingDropdown.onValueChanged.AddListener(value =>
            {
                hdCameraData.antialiasing = (HDAdditionalCameraData.AntialiasingMode)value;
                ActiveAntialiasingOption((HDAdditionalCameraData.AntialiasingMode)value);
            });

            taaQualityDropdown.onValueChanged.AddListener(value =>
            {
                hdCameraData.TAAQuality = (HDAdditionalCameraData.TAAQualityLevel)value;
            });

            taaSharpenScrollbar.onValueChanged.AddListener(value =>
            {
                hdCameraData.taaSharpenStrength = value * 2;
            });

            smaaQualityDropdown.onValueChanged.AddListener(value =>
            {
                hdCameraData.SMAAQuality = (HDAdditionalCameraData.SMAAQualityLevel)value;
            });

            goTitleButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.StopAll(); 
                DeActivationActions();
                SceneManager.LoadScene(StageNames.Title.ToString());
                Utils.MoveTime();
            });

            gameRestartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(gameObject.scene.name);
                SoundManager.Instance.StopAll();
                Utils.LockCursor();
                Utils.MoveTime();
            });

            disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
                Utils.LockCursor();
                Utils.MoveTime();
            });

            resetButton.onClick.AddListener(() =>
            {
                ResetSetting();
            });

            soundScrollbar.onValueChanged.AddListener(value =>
            {
                UIManager.Instance.soundAction(value);
            });

            mouseScrollbar.onValueChanged.AddListener(value =>
            {
                UIManager.Instance.sensitivityAction(value);
            });
        }

        public override void UpdateActions()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Utils.LockCursor();
                UIManager.Instance.DeActivationPanel(panelId);
                Utils.MoveTime();
            }
        }
        private void SetScreenMode(ScreenMode screenMode)
        {
            switch (screenMode)
            {
                case ScreenMode.FullScreen:
                    ScreenManager.Instance.SetFullScreen();
                    break;
                case ScreenMode.WindowScreen:
                    ScreenManager.Instance.SetFullScreen();
                    break;
            }
        }

        private void SetResolution(string text)
        {
            string[] texts = text.Split('x');

            int width = int.Parse(texts[0]);
            int height = int.Parse(texts[1]);

            ScreenManager.Instance.SetResolution(width, height);
        }

        private void ResetSetting()
        {
            SoundVO soundVO = new SoundVO(0.8f);
            MouseVO mouseVO = new MouseVO(0.8f);
            GraphicVO graphicVO = new GraphicVO(1, 1, 0.8f, 1, 1, 1, 2, 1, 0.5f, 1);

            Utils.VOToJson(_soundPath, soundVO);
            Utils.VOToJson(_mousePath, mouseVO);
            Utils.VOToJson(_graphicPath, graphicVO);

            soundScrollbar.value = 0.8f;
            mouseScrollbar.value = 0.8f;
            UIManager.Instance.soundAction(0.8f);
            UIManager.Instance.sensitivityAction(0.8f);

            graphicQualityDropdown.value = 1;
            shadowDropdown.value = 1;
            gammaScrollbar.value = 0.8f;
            bloomDropdown.value = 1;
            lightingDropdown.value = 1;
            motionBlurDropdown.value = 1;
            antialiasingDropdown.value = 2;
            taaQualityDropdown.value = 1;
            taaSharpenScrollbar.value = 0.25f;
            smaaQualityDropdown.value = 1;

            tonemapping.gamma.value = 1f;
            bloom.quality.value = 1;
            ssr.quality.value = 1;
            globalillumination.quality.value = 1;
            ambientOcclusion.quality.value = 1;
            motionBlur.quality.value = 1;
            hdCameraData.renderingPathCustomFrameSettings.lodBiasQualityLevel = 1;
            hdCameraData.renderingPathCustomFrameSettings.maximumLODLevelQualityLevel = 1;
            hdCameraData.renderingPathCustomFrameSettings.sssQualityLevel = 1;
            hdCameraData.antialiasing = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
            hdCameraData.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Medium;
            hdCameraData.taaSharpenStrength = 0.5f;
            hdCameraData.SMAAQuality = HDAdditionalCameraData.SMAAQualityLevel.Medium;
        }

        private void ActiveAntialiasingOption(HDAdditionalCameraData.AntialiasingMode aa)
        {
            switch (aa)
            {
                case HDAdditionalCameraData.AntialiasingMode.None:
                case HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing:
                    taaQualityDropdown.transform.parent.gameObject.SetActive(false);
                    taaSharpenScrollbar.transform.parent.gameObject.SetActive(false);
                    smaaQualityDropdown.transform.parent.gameObject.SetActive(false);
                    break;
                case HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing:
                    taaQualityDropdown.transform.parent.gameObject.SetActive(true);
                    taaSharpenScrollbar.transform.parent.gameObject.SetActive(true);
                    smaaQualityDropdown.transform.parent.gameObject.SetActive(false);
                    break;
                case HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing:
                    taaQualityDropdown.transform.parent.gameObject.SetActive(false);
                    taaSharpenScrollbar.transform.parent.gameObject.SetActive(false);
                    smaaQualityDropdown.transform.parent.gameObject.SetActive(true);
                    break;
            }
        }

        private int GetResolutionIndex(int width, int height)
        {
            string w = width.ToString();
            string h = height.ToString();
            string t = w + 'x' + h;

            return resolutionDropdown.options.IndexOf(resolutionDropdown.options.Find(x => x.text == t));
        }
    }
}