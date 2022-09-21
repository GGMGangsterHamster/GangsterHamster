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
        private string _soundPath = "SettingValue/Sound.json";
        private string _sensitivityPath = "SettingValue/Sensitivity.json";

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

        [Header("HDRP Settings")]
        public VolumeProfile globalVolume;


        public Tonemapping tonemapping;
        public Bloom bloom;
        public ScreenSpaceReflection ssr;
        public GlobalIllumination globalillumination;
        public AmbientOcclusion ambientOcclusion;
        public MotionBlur motionBlur;

        private HDAdditionalCameraData hdCameraData;


        public override void ActivationActions()
        {
            // 여기서 스크롤바들의 값을 초기화 시켜줌
            SoundVO soundVO = Utils.JsonToVO<SoundVO>(_soundPath);
            SensitivityVO sensitivityVO = Utils.JsonToVO<SensitivityVO>(_sensitivityPath);

            soundScrollbar.value = soundVO.master;
            mouseScrollbar.value = sensitivityVO.sensitivity;
        }

        public override void DeActivationActions()
        {

            SoundVO soundVO = new SoundVO(soundScrollbar.value);
            SensitivityVO sensitivityVO = new SensitivityVO(mouseScrollbar.value);

            Utils.VOToJson(_soundPath, soundVO);
            Utils.VOToJson(_sensitivityPath, sensitivityVO);
        }

        public override void InitActions()
        {
            panelId = 4;

            Debug.Log(globalVolume.TryGet(out tonemapping));
            Debug.Log(globalVolume.TryGet(out bloom));
            Debug.Log(globalVolume.TryGet(out ssr));
            Debug.Log(globalVolume.TryGet(out globalillumination));
            Debug.Log(globalVolume.TryGet(out ambientOcclusion));
            Debug.Log(globalVolume.TryGet(out motionBlur));

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
    }

}