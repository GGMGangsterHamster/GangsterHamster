using UnityEngine;
using Tween;
using System.Collections;
using Player.Status;

namespace Animation.Camera
{
   public class CameraFov : MonoBehaviour
   {
      private UnityEngine.Camera _mainCam = null;
      private UnityEngine.Camera MainCam
      {
         get
         {
            if (_mainCam == null)
            {
               _mainCam = GetComponent<UnityEngine.Camera>();
            }

            return _mainCam;
         }
      }

      [SerializeField] private float _foVChangeDuration = 0.2f;

      private float _defaultFov = 83.0f;
      private float _runFoV = 88.0f;
      private float _eP = 0.0f;

      private void Update()
      {
         float step = 1.0f * _foVChangeDuration * Time.deltaTime;
         
         _eP = Mathf.Clamp01(PlayerStatus.IsRunning ?
                             _eP + _foVChangeDuration * Time.deltaTime :
                             _eP - _foVChangeDuration * Time.deltaTime);

         MainCam.fieldOfView = Mathf.Lerp(_defaultFov, _runFoV, _eP);
      }
   }
}