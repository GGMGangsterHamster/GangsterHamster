using Setting.VO;
using UI.Screen;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class OptionAction : UIAction
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

        [Header("Buttons")]
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

            if (soundVO != null)
                soundScrollbar.value = soundVO.master;
            if (mouseVO != null)
                mouseScrollbar.value = mouseVO.sensitivity;

            if (screenVO != null)
            {
                screenModeDropdown.value = screenVO.isFullScreen ? 0 : 1;
                resolutionDropdown.value = GetResolutionIndex(screenVO.width, screenVO.height);
            }

            if (graphicVO != null)
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

                graphicQualityDropdown.value = graphicVO.graphicQuality;
                shadowDropdown.value = graphicVO.shadow;
                gammaScrollbar.value = graphicVO.gamma;
                bloomDropdown.value = graphicVO.bloom;
                lightingDropdown.value = graphicVO.lighting;
                motionBlurDropdown.value = graphicVO.motionBlur;
            }
        }

        public override void DeActivationActions()
        {
            SoundVO soundVO = new SoundVO(soundScrollbar.value);
            MouseVO mouseVO = new MouseVO(mouseScrollbar.value);
            GraphicVO graphicVO = new GraphicVO(graphicQualityDropdown.value, shadowDropdown.value, gammaScrollbar.value, bloomDropdown.value, lightingDropdown.value, motionBlurDropdown.value);

            Utils.VOToJson(_soundPath, soundVO);
            Utils.VOToJson(_mousePath, mouseVO);
            Utils.VOToJson(_graphicPath, graphicVO);
        }

        public override void InitActions()
        {
            panelId = 4;

            globalVolume.TryGet(out tonemapping);
            globalVolume.TryGet(out bloom);
            globalVolume.TryGet(out ssr);
            globalVolume.TryGet(out globalillumination);
            globalVolume.TryGet(out ambientOcclusion);
            globalVolume.TryGet(out motionBlur);

            hdCameraData = Camera.main.GetComponent<HDAdditionalCameraData>();

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
                motionBlur.quality.value = value;
            });

            graphicQualityDropdown.onValueChanged.AddListener(value =>
            {
                hdCameraData.renderingPathCustomFrameSettings.lodBiasQualityLevel = value;
                hdCameraData.renderingPathCustomFrameSettings.maximumLODLevelQualityLevel = value;
                hdCameraData.renderingPathCustomFrameSettings.sssQualityLevel = value;
            });

            disableButton.onClick.AddListener(() =>
            {
                UIManager.Instance.DeActivationPanel(panelId);
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.DeActivationPanel(panelId);
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
            GraphicVO graphicVO = new GraphicVO(1, 1, 0.8f, 1, 1, 1);

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

            tonemapping.gamma.value = 1f;
            bloom.quality.value = 1;
            ssr.quality.value = 1;
            globalillumination.quality.value = 1;
            ambientOcclusion.quality.value = 1;
            motionBlur.quality.value = 1;
            hdCameraData.renderingPathCustomFrameSettings.lodBiasQualityLevel = 1;
            hdCameraData.renderingPathCustomFrameSettings.maximumLODLevelQualityLevel = 1;
            hdCameraData.renderingPathCustomFrameSettings.sssQualityLevel = 1;
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