using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Bezier;

namespace Objects.Movement
{
    public class ObjectMovementManager : Singleton<ObjectMovementManager>, ISingletonObject
    {
        /// <summary>
        /// 오브젝트를 전달된 포지션으로 베지어 곡선을 따라 이동
        /// </summary>
        /// <param name="target">이동할 오브젝트</param>
        /// <param name="duration">이동 시간</param>
        /// <param name="bezier">베지어 곡선 오브젝트</param>
        /// <param name="mono">Coroutine 실행 용</param>
        /// <param name="callback">목적지 도착시 호출</param>
        /// <param name="reverse">역재생</param>
        public void BezierMove(Transform target, float duration, BezierObject bezier, MonoBehaviour mono,
                                        Action callback = null, bool reverse = false)
        {
            mono.StartCoroutine(BezierMove(target, duration, bezier, callback, reverse));
        }


        IEnumerator BezierMove(Transform target, float duration, BezierObject bezier, Action callback = null, bool reverse = false)
        {
            float t = reverse ? 1.0f : 0.0f;
            bool arrived = false;

            while(!arrived)
            {
                target.position = bezier.CoordAtT(t);
                t = reverse ? (t - Time.deltaTime / duration) : (t + Time.deltaTime / duration);
                
                if(reverse) {
                    arrived = t <= 0.0f;
                }
                else {
                    arrived = t >= 1.0f;
                }
                
                yield return null;
            }

            target.position = reverse ? bezier.startPoint : bezier.endPoint;
            callback?.Invoke();
        }
    }
}