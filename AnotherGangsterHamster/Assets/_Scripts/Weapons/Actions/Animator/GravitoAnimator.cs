using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapon.Animation.GravitoAnimation
{
    public class GravitoAnimator : MonoBehaviour
    {
        private Transform _mainCameraTransform;
        private Transform MainCameraTransform
        {
            get
            {
                if (_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }

        enum GravitoAnimeStatus // 현재 애니메이션 스테이터스
        {
            Idle,
            Move,
            Reset,
            Using,
            Sorting
        }

        public float resetMultiply;

        public Action sticklyAction;

        private GravitoAnimeStatus _curStatus = GravitoAnimeStatus.Idle;    // 지금 애니메이션 스테이터스

        private Gravito _gravito;

        private Vector3 start;
        private Vector3 end;
        private float moveSpeed;
        private float _curTime;

        private bool isEnd = false;
        private bool isReset = true;

        private void Awake()
        {
            _gravito = GetComponent<Gravito>();
        }

        public void FireAnime(Vector3 start, Vector3 end, float moveSpeed)
        {
            InitAnime(start, end, moveSpeed, GravitoAnimeStatus.Move);
            isReset = false;
            isEnd = false;
        }

        public void ResetAnime(Vector3 start, Vector3 end, float moveSpeed)
        {
            InitAnime(start, end, moveSpeed, GravitoAnimeStatus.Reset);
            isReset = true;
            isEnd = false;
        }
        public bool isStopedMoving()
        {
            return _curStatus == GravitoAnimeStatus.Idle || isEnd;
        }


        private void Update()
        {
            switch (_curStatus)
            {
                case GravitoAnimeStatus.Idle:
                    if(isReset)
                    {
                        transform.position = _gravito.GravitoHandPosition;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MainCameraTransform.forward), 0.5f);
                    }
                    break;
                case GravitoAnimeStatus.Move:
                    _curTime += Time.deltaTime * moveSpeed;

                    if (_curTime >= Vector3.Distance(start, end))
                    {
                        // 발사 위치로 이동 완료
                        transform.position = end;
                        _curStatus = GravitoAnimeStatus.Idle;
                        sticklyAction?.Invoke();
                        _curTime = 0;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(start, end, _curTime / Vector3.Distance(start, end));
                    }
                    break;
                case GravitoAnimeStatus.Reset:
                    _curTime += Time.deltaTime * moveSpeed * resetMultiply;

                    if (_curTime >= Vector3.Distance(start, _gravito.GravitoHandPosition))
                    {
                        transform.position = _gravito.GravitoHandPosition;
                        _curStatus = GravitoAnimeStatus.Idle;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(start, _gravito.GravitoHandPosition, _curTime / Vector3.Distance(start, _gravito.GravitoHandPosition));
                    }
                    break;
            }
        }

        private void InitAnime(Vector3 start, Vector3 end, float moveSpeed, GravitoAnimeStatus status)
        {
            _curTime = 0;

            this.start = start;
            this.end = end;
            this.moveSpeed = moveSpeed;
            _curStatus = status;
        }
    }
}

