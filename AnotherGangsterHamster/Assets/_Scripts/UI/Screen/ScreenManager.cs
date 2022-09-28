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

        private bool _isFullScreen = true;

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

            SaveScreenSetting();
        }

        private void SaveScreenSetting()
        {
            if(_isFullScreen)
                UnityEngine.Screen.SetResolution(_width, _height, FullScreenMode.FullScreenWindow);
            else
                UnityEngine.Screen.SetResolution(_width, _height, FullScreenMode.Windowed);
            //UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
            ScreenVO vo = new ScreenVO(_isFullScreen, _width, _height);
            Utils.VOToJson(_screenPath, vo);
        }

        private void LoadScreenSetting()
        {
            ScreenVO vo = Utils.JsonToVO<ScreenVO>(_screenPath);

            if(vo != null)
            {
                _isFullScreen = vo.isFullScreen;
                _width = vo.width;
                _height = vo.height;
            }

            if (_isFullScreen)
                UnityEngine.Screen.SetResolution(_width, _height, FullScreenMode.FullScreenWindow);
            else
                UnityEngine.Screen.SetResolution(_width, _height, FullScreenMode.Windowed);
        }
    }
}
