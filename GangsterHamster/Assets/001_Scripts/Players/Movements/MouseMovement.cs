using UnityEngine;
using Commands.Movement;

namespace Player.Mouse
{
   public class MouseMovement : MonoBehaviour, IMouseDeltaRecvable
   {
      private Transform _camTrm = null;
      private Transform CamTrm 
      {
         get
         {
            if(_camTrm == null)
            {
               _camTrm = Camera.main.transform;
            }

            return _camTrm;
         }
      }

      private Transform _playerTrm = null;
      private Transform PlayerTrm 
      {
         get
         {
            if (_playerTrm == null)
            {
               _playerTrm = GameManager.Instance.Player.transform;
            }

            return _playerTrm;
         }
      }

      #region Mouse delta
      public void OnMouseX(float x)
      {
         // 4원수는 교환 법칙이 성립되지 않는대요
         // 4원수 * 곱하고자 하는 수 = local
         // 곱하고자 하는 수 * 4원수 = world
         PlayerTrm.rotation = PlayerTrm.rotation * Quaternion.Euler(0.0f, x * PlayerValues.mouseSpeed, 0.0f);
      }

      float rotY = 0.0f;
      public void OnMouseY(float y, bool includingMouseSpeed = true)
      {
         rotY += -y * (includingMouseSpeed ? PlayerValues.mouseSpeed : 1);
         rotY = Mathf.Clamp(rotY, -90f, 90f);

         CamTrm.transform.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
      }
      #endregion

      public void SetMouseY(float y)
      {
         rotY = y;

         CamTrm.transform.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
      }
   }
}