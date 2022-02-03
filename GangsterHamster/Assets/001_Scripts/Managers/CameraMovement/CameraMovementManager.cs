using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Bezier;

namespace Objects.Movement.Camera
{

    /// <summary>
    /// 지정한 목적지로 카메라를 이동시켜주는 클레스
    /// </summary>
    public class CameraMovementManager : Singleton<CameraMovementManager>, ISingletonObject
    {

        #region bool _isMoving
        private bool _isMoving = false;
        /// <summary>
        /// 카메라가 이동중인지 나타냄
        /// </summary>
        public bool IsMoving
        {
            get { return _isMoving; }
        }
        #endregion // bool _isMoving

        #region Transform _mainCam
        private Transform _mainCam = null;
        /// <summary>
        /// 메인 카메라
        /// </summary>
        public Transform MainCam {
            get { return _mainCam; }
        }
        #endregion // Transform _mainCam

        public CameraMovementManager()
        {
            _mainCam = UnityEngine.Camera.main.transform;
        }

        #region Bezier curve movement
        /// <summary>
        /// 카메라를 전달된 포지션으로 베지어 곡선을 통해 이동
        /// </summary>
        /// <param name="duration">지점과 지점의 이동 시간</param>
        /// <param name="bezier">이동 시간</param>
        /// <param name="mono">Coroutine 실행 용</param>
        /// <param name="returnToDefaultPosition">시작 위치로 되돌리는지 여부</param>
        /// <param name="targetCamera">이동할 카메라 (Default: Main Camrea)</param>
        /// <param name="onArrived">목적지 도착시 호출</param>
        /// <param name="onReturned">원위치 도착시 호출</param>
        public void BezierMove(float duration, BezierObject bezier, MonoBehaviour mono, bool returnToDefaultPosiiton = true, Transform targetCamera = null, Action onArrived = null, Action onReturned = null)
        {
            if(!Moveable()) return;

            _isMoving = true;

            if(returnToDefaultPosiiton)
            {
                ObjectMovementManager.Instance.BezierMove(targetCamera ?? MainCam, duration, bezier, mono, () => {
                    onArrived?.Invoke();

                    ObjectMovementManager.Instance.BezierMove(targetCamera ?? MainCam, duration, bezier, mono, () => {
                        _isMoving = false;
                        onReturned?.Invoke();
                    }, true);
                });
            }
            else
            {
                ObjectMovementManager.Instance.BezierMove(targetCamera == null ? MainCam : targetCamera, duration, bezier, mono, () => {
                    _isMoving = false;
                    onArrived?.Invoke();
                });
            }
        }
        #endregion // Bezier curve movement

        #region Linear curve movement
        /// <summary>
        /// 오브젝트를 전달된 포지션으로 선형 이동
        /// </summary>
        /// <param name="targetCamera">이동할 오브젝트</param>
        /// <param name="duration">이동 시간</param>
        /// <param name="from">시작점</param>
        /// <param name="to">목적지</param>
        /// <param name="mono">Coroutine 실행 용</param>
        /// <param name="callback">목적지 도착시 호출</param>
        public void LinearMove(float duration, Vector3 from, Vector3 to, MonoBehaviour mono, Transform targetCamera = null, Action callback = null)
        {
            if(!Moveable()) return;
            _isMoving = true;
            ObjectMovementManager.Instance.LinearMove(targetCamera ?? MainCam, duration, from, to, mono, () => {
                _isMoving = false;
                callback?.Invoke();
            });
        }
        #endregion // Linear curve movement


        private bool Moveable()
        {
            if(_isMoving) 
            {
                Logger.Log("카메라 이미 이동중", LogLevel.Warning);
                return false;
            }

            return true;
        }

    }
}