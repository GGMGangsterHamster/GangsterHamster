using System;
using UnityEngine;

namespace Characters.Player.Mouse
{

    public class Mouse : MonoBehaviour, IMousedeltaRecvable
    {
        private Transform _camTrm = null;
        private Transform CamTrm
        {
            get
            {
                if (_camTrm == null)
                    _camTrm = Camera.main.transform;

                return _camTrm;
            }
        }

        private Transform _playerTrm = null;
        private Transform PlayerTrm
        {
            get
            {
                if (_playerTrm == null)
                    _playerTrm = GameObject.FindWithTag("PLAYER_BASE").transform;

                return _playerTrm;
            }
        }


        // For Y rotaion clamping
        public float rotY = 0.0f;

        public void OnMouseX(float x, Action<float> callback)
        {
            if (!PlayerStatus.Moveable) return;
            
            callback(x * (PlayerValues.CanMouseMove ? PlayerValues.MouseSpeed : 0));
        }

        public void OnMouseY(float y, bool includingMouseSpeed = true)
        {
            if (!PlayerStatus.Moveable) return;
            // 마우스 감도 사용할 지
            rotY -= y * (includingMouseSpeed ? (PlayerValues.CanMouseMove ? PlayerValues.MouseSpeed : 0) : 1.0f);
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            CamTrm.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
        }
    }
}