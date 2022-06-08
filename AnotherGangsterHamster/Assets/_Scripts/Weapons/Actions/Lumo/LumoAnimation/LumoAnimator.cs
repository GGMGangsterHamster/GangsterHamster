using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Weapon.Animation.Lumo
{
    public class LumoAnimator : MonoBehaviour
    {
        enum LumoAnimeStatus // 현재 애니메이션 스테이터스
        {
            Idle,
            Start,
            Using,
            Stop
        }

        [SerializeField] private float startAnimeDelayTime = 0.5f;                    // 애니메이션을 시작하는데 생기는 딜레이
        [SerializeField] private float stopAnimeDelayTime = 0.5f;                    // 애니메이션을 멈추는데 생기는 딜레이
        [SerializeField] private float rotSpeed = 1f;                                 // 돌아가는 회전 속도

        public UnityEvent StartAnimationCallback;
        public UnityEvent StopAnimationCallback;

        private List<Vector3> _partRotDirList = new List<Vector3>();    // 파츠마다의 애니메이션 회전 값
        private List<Transform> _partTrmList = new List<Transform>();
        private LumoAnimeStatus _curStatus = LumoAnimeStatus.Idle;    // 지금 애니메이션 스테이터스

        private float _curTime;                                     // 시간 계산을 위한 변수
        private int childCount;                                     // 자식의 수 - 그냥 많이 쓰여서 따로 변수로 뺌

        private void Awake()
        {
            childCount = transform.childCount;
            // 시작시 랜덤으로 파츠마다의 랜덤 회전 값 지정
            for (int i = 0; i < childCount; i++)
            {
                _partRotDirList.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                _partTrmList.Add(transform.GetChild(i));
            }
        }

        public void StartAnimation()
        {
            _curTime = 0;
            _curStatus = LumoAnimeStatus.Start;
        }

        public void StopAnimation()
        {
            _curTime = stopAnimeDelayTime;
            _curStatus = LumoAnimeStatus.Stop;
        }

        private void Update()
        {
            #region 임시로 넣은 Input.GetKeyDown()
            if (Input.GetKeyDown(KeyCode.U))
                StartAnimation();
            if (Input.GetKeyDown(KeyCode.P))
                StopAnimation();
            #endregion

            // Idle 은 아무것도 하지 않는다!
            switch (_curStatus)
            {
                case LumoAnimeStatus.Start:
                    if(_curTime >= startAnimeDelayTime)
                    {
                        // 시작 애니메이션 끝
                        _curStatus = LumoAnimeStatus.Using;
                        StartAnimationCallback?.Invoke();
                    }
                    _curTime += Time.deltaTime;
                    RotationParts(_curTime / startAnimeDelayTime * Time.deltaTime * rotSpeed);

                    break;
                case LumoAnimeStatus.Stop:
                    if (_curTime <= 0)
                    {
                        // 스탑 애니메이션 끝
                        _curStatus = LumoAnimeStatus.Idle;
                        StopAnimationCallback?.Invoke();
                    }
                    _curTime -= Time.deltaTime;
                    RotationParts(_curTime / stopAnimeDelayTime * Time.deltaTime * rotSpeed);

                    break;
                case LumoAnimeStatus.Using:
                    RotationParts(rotSpeed * Time.deltaTime);
                    break;
            }
        }

        // 들어온 스피드에 따라서 파츠들을 돌리는 함수
        private void RotationParts(float rotSpeed)
        {
            for(int i = 0; i < childCount; i++)
            {
                _partTrmList[i].rotation *= Quaternion.Euler(_partRotDirList[i] * rotSpeed);
            }
        }
    }
}