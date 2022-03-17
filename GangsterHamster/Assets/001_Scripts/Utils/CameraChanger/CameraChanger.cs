using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Camera
{
    public class CameraChanger : Singleton<CameraChanger>, ISingletonObject
    {
        private Dictionary<string, UnityEngine.Camera> _cameraDictionary;
        private UnityEngine.Camera                     _currentCamera;


        public CameraChanger()
        {
            _cameraDictionary = new Dictionary<string, UnityEngine.Camera>();
        }
      
         /// <summary>
         /// 카메라를 관리 리스트에 추가합니다.
         /// </summary>
         /// <param name="key">식별자</param>
         /// <param name="camera">추가할 카메라</param>
        public void AddToList(string key, UnityEngine.Camera camera)
        {
            if (_cameraDictionary.ContainsKey(key))
            {
                Logger.Log($"CameraChanger > 중복 key: {key}", LogLevel.Error);
                return;
            }

            _cameraDictionary.Add(key, camera);
        }

         /// <summary>
         /// 카메라를 활성화합니다.
         /// </summary>
         /// <param name="key">식별자</param>
         /// <returns>활성화 된 카메라</returns>
        public UnityEngine.Camera EnableCamera(string key,
                                          bool overrideCurrentCamera = false)
        {
            if (!ContainsKey(key))
                return null;

            UnityEngine.Camera target = _cameraDictionary[key];

            // 카메라를 바꿈
            target.gameObject.SetActive(true);
            _currentCamera.gameObject.SetActive(false);
            _currentCamera = target;

            return target;
        }

         /// <summary>
         /// 카메라를 비활성화합니다.
         /// </summary>
         /// <param name="key">식별자</param>
        public void DisableCamrea(string key)
        {
            if (!ContainsKey(key)) return;

            _currentCamera.gameObject.SetActive(false);
        }

         /// <summary>
         /// _cameraDictionary.ContainsKey(key);
         /// </summary>
         /// <param name="key">식별자</param>
         /// <returns>flase when _cameraDictionary does not contains request key</returns>
        private bool ContainsKey(string key)
        {
            if (!_cameraDictionary.ContainsKey(key))
            {
                Logger.Log($"CamreaChanger > {key} 를 찾을 수 없습니다.");
                return false;
            }
            return true;
        }


    }
}