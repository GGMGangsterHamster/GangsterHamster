using UnityEngine;
using Tween;
using System.Collections;
using Player.Status;

namespace Animation.Camera
{
   public class CameraFov : MonoBehaviour
   {
      [SerializeField] private float _foVChangeDuration = 0.2f;
      [SerializeField] private float _defaultFov = 83.0f;
      [SerializeField] private float _runFoV = 88.0f;


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

      private float _eP = 0.0f;

      private void Update()
      {
         RunningFov();
      }

      private void RunningFov()
      {
         float step = (1.0f / _foVChangeDuration) * Time.deltaTime;

         _eP = Mathf.Clamp01(PlayerStatus.IsRunning && PlayerStatus.IsMoving ?
                             _eP + step :
                             _eP - step);

         MainCam.fieldOfView = Mathf.Lerp(_defaultFov, _runFoV, _eP);
      }
   }
}