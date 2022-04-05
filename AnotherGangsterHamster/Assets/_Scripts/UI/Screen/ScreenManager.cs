using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screen
{
    public class ScreenManager : Singleton<ScreenManager>
    {
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

        private bool _isFullScreen = true;

        private int _width = 1920;
        private int _height = 1080;

        public void SetFullScreen()
        {
            _isFullScreen = true;

            UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
        }

        public void SetWindowScreen()
        {
            _isFullScreen = false;

            UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
        }

        public void SetResolution(int width, int height)
        {
            _width = width;
            _height = height;

            UnityEngine.Screen.SetResolution(_width, _height, _isFullScreen);
            GamePlayCanvasScaler.referenceResolution = new Vector2(_width, _height);
        }
    }
}
