using UnityEngine;
using Commands.Movement;

namespace Player.Mouse
{
    public class MouseMovement : MonoBehaviour, IMouseDeltaRecvable
    {

        private Transform camTrm = null;
        private Transform playerTrm = null;

        private void Start() {
            camTrm = Camera.main.transform;
            playerTrm = GameManager.Instance.player.transform;
        }

        #region Mouse delta
        public void OnMouseX(float x)
        {
            // 4원수는 교환 법칙이 성립되지 않는대요
            // 4원수 * 곱하고자 하는 수 = local
            // 곱하고자 하는 수 * 4원수 = world
            playerTrm.rotation = playerTrm.rotation * Quaternion.Euler(0.0f, x * PlayerValues.Instance.mouseSpeed, 0.0f);
        }

        float rotY = 0.0f;
        public void OnMouseY(float y, bool includingMouseSpeed = true)
        {
            rotY += -y * (includingMouseSpeed ? PlayerValues.Instance.mouseSpeed : 1);
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            camTrm.transform.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
        }
        #endregion

        public void SetMouseY(float y) {
            rotY = y;

            camTrm.transform.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
        }
    }
}