using System;

namespace Characters.Player.Mouse
{
    public interface IMousedeltaRecvable
    {
        public void OnMouseX(float x, Action<float> callback);

        /// <summary>
        /// 마우스 Y 의 delta 를 전달
        /// </summary>
        public void OnMouseY(float y, bool includingMouseSpeed = true);
    }
}