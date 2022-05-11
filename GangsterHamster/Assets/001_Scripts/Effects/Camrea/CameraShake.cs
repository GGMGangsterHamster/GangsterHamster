using Tween;
using UnityEngine;
using System.Collections.Generic;

namespace Effects.Camrea
{   
    public class CameraShake : MonoSingleton<CameraShake>
    {
        private Dictionary<uint, float> _shakeDurationCheckDictionary = new Dictionary<uint, float>(); // TODO: 다른 클레스로 빼야 함
        private uint _index = 0;

        /// <summary>
        /// 카메라를 흔듭니다.
        /// </summary>
        /// <param name="x">-x ~ x</param>
        /// <param name="y">-y ~ y</param>
        /// <param name="duration">지속 시간</param>
        public void Shake(float x, float y, float duration) {
        
            uint index = _index++;
            _shakeDurationCheckDictionary.Add(index, 0.0f);
            float step = Mathf.PI / duration;

            ValueTween.To(this, () => {
                float amplitude = _shakeDurationCheckDictionary[index] += step * Time.deltaTime;
                Vector2 delta = new Vector2(Random.Range(-x, x), Random.Range(-y, y)) * Mathf.Sin(amplitude);
                transform.localPosition = delta;
            }, () => {
                return _shakeDurationCheckDictionary[index] >= Mathf.PI;
            }, () => {
                transform.localPosition = Vector3.zero;
                _shakeDurationCheckDictionary.Remove(index);
            });
        }
    }
}