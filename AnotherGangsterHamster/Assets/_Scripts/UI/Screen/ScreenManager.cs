using Setting.VO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screen
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        private string _screenPath = "SettingValue/Screen.json";

        private CanvasScaler _gamePlayCanvas;
        
        private CanvasScaler GamePlayCanvasScaler
        {
            get
            {
                if(_gamePlayCanvas == null)
                {
                    _gamePlayCanvas = GameObject.Find("GamePlayCanvas").GetComponent<CanvasScaler>();
                }

                return _gamePlayCanvas;
            }
        }

        private bool _isFullScreen;

        private int _width = 1920;
        private int _height = 1080;

        private new void Awake()
        {
            base.Awake();
            LoadScreenSetting();
        }

        public void SetFullScreen()
        {
            _isFullScreen = true;

            SaveScreenSetting();
        }

        public void SetWindowScreen()
        {
            _isFullScreen = false;

            SaveScreenSetting();
        }

        public void SetResolution(int width, int height)
        {
            _width = width;
            _height = height;

            GamePlayCanvasScaler.referenceResolution = new Vector2(_width, _height);
            SaveScreenSetting();
        }

        private void SaveScreenSetting()
        {
            UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
            ScreenVO vo = new ScreenVO(_isFullScreen, _width, _height);
            Utils.VOToJson(_screenPath, vo);
        }

        private void LoadScreenSetting()
        {
            ScreenVO vo = Utils.JsonToVO<ScreenVO>(_screenPath);

            _isFullScreen = vo.isFullScreen;
            _width = vo.width;
            _height = vo.height;

            UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
        }
    }
}
