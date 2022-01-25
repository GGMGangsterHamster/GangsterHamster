using UnityEngine;

namespace Objects.Camera
{

    public class AttachCameraToObject : MonoSingleton<AttachCameraToObject>
    {

        // 카메라가 고정될 Transform
        private Transform _attachPos = null;
        

        #region bool IsAttached
        private bool _isAttached = false;
        /// <summary>
        /// 카메라 고정 여부
        /// </summary>
        public bool IsAttached { get => _isAttached; }
        #endregion // bool IsAttached

        /// <summary>
        /// 카메라를 pos 에 고정시킵니다.
        /// </summary>
        /// <param name="pos">고정할 오브젝트의 Transform</param>
        public void SetAttachPosition(Transform pos)
        {
            _isAttached = true;
            _attachPos = pos;
            transform.SetParent(pos);
            transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 카메라 고정을 해제합니다.
        /// </summary>
        public void FreeCamera()
        {
            _isAttached = false;
            transform.SetParent(null);
        }
    }
}