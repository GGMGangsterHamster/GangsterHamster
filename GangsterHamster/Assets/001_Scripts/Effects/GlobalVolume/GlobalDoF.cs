using Tween;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Effects.Global
{
    public class GlobalDoF : Effects<GlobalDoF, DepthOfField>
    {
        /// <summary>
        /// 효과를 높혀 나갑니다.
        /// </summary>
        /// <param name="startValue">시작 값</param>
        /// <param name="endValue">종료 값</param>
        /// <param name="duration">기간 (초)</param>
        public void Increase(float startValue, float endValue, float duration, Action callback = null)
        {
            float step = (endValue - startValue) / duration;

            Tween(() => {
                _volume.weight = startValue;
            }, () => {
                _volume.weight += step * Time.deltaTime;
            }, () => _volume.weight >= endValue, callback);
        }

        /// <summary>
        /// 효과를 줄여 나갑니다.
        /// </summary>
        /// <param name="startValue">시작 값</param>
        /// <param name="endValue">종료 값</param>
        /// <param name="duration">기간 (초)</param>
        public void Decrease(float startValue, float endValue, float duration, Action callback = null)
        {
            float step = (endValue - startValue) / duration;

            Tween(() => {
                _volume.weight = startValue;
            }, () => {
                _volume.weight += step * Time.deltaTime;
            }, () => _volume.weight <= endValue, callback);
        }

    }
}