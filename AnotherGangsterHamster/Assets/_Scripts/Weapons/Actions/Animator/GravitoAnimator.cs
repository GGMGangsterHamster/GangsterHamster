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

        public GameObject rotEtcObj;
        public List<GameObject> rotCainObjs = new List<GameObject>();
        public Vector3 rotDir;
        public float rotSpeed;
        public float usingAnimeDelay;
        public float resetMultiply;

        public Action sticklyAction;

        private GravitoAnimeStatus _curStatus = GravitoAnimeStatus.Idle;    // 지금 애니메이션 스테이터스

        private Gravito _gravito;

        private Vector3 start;
        private Vector3 end;
        private float moveSpeed;
        private float _curTime;
        private float _softMoving = 0;

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
        public void UsingAnime()
        {
            // Using -> 계속 유지함
            InitAnime(Vector3.zero, Vector3.zero, 0, GravitoAnimeStatus.Using);
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
                case GravitoAnimeStatus.Using:
                    _curTime += Time.deltaTime / usingAnimeDelay * _softMoving;
                    _softMoving += Time.deltaTime;

                    if (_curTime >= Vector3.Distance(start, end))
                    {
                        // 조금 튀어나옴
                        RotationEtc(rotSpeed * Time.deltaTime);
                        isEnd = true;
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

                        rotEtcObj.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                        for (int i = 0; i < rotCainObjs.Count; i++)
                            rotCainObjs[i].transform.localRotation = Quaternion.Euler(-90, 0, 0);
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

        // 들어온 스피드에 따라서 파츠들을 돌리는 함수
        private void RotationEtc(float rotSpeed)
        {
            rotEtcObj.transform.rotation *= Quaternion.Euler(rotDir * rotSpeed);

            for (int i = 0; i < rotCainObjs.Count; i++)
                rotCainObjs[i].transform.localRotation *= Quaternion.Euler(new Vector3(0.5f, 0.5f, 0.5f) * rotSpeed / 3);
        }
    }
}

