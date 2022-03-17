using Tween;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Effects.Global
{
    public class GlobalChromaticAberration : Effects<GlobalChromaticAberration, ChromaticAberration>
    {

        /// <summary>
        /// 값을 설정합니다.
        /// </summary>
        /// <param name="value">값 (0 ~ 1)</param>
        public void Set(float value)
        {
            _globalVolume.intensity.value = value;
        }

        /// <summary>
        /// 효과를 높혀 나갑니다.
        /// </summary>
        /// <param name="startValue">시작 값</param>
        /// <param name="endValue">종료 값</param>
        /// <param name="duration">기간 (초)</param>
        public void Increase(float startValue, float endValue, float duration, Action callback = null)
        {
            float step = (endValue - startValue) / duration;

            Tween(() =>
            {
                _globalVolume.intensity.value = startValue;
            }, () =>
            {
                _globalVolume.intensity.value += step * Time.deltaTime;
            }, () => _globalVolume.intensity.value >= endValue, callback);
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

            Tween(() =>
            {
                _globalVolume.intensity.value = startValue;
            }, () =>
            {
                _globalVolume.intensity.value += step * Time.deltaTime;
            }, () => _globalVolume.intensity.value <= endValue, callback);
        }


    }
}