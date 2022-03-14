using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Effects.Global
{
    public class GlobalChromaticAberration : MonoBehaviour
    {
        private Volume _volume;
        private ChromaticAberration _chromaticAberration;

        private void Awake() {

            if(!_volume.sharedProfile.TryGet<ChromaticAberration>(out var _chromaticAberration)) {
                _chromaticAberration = _volume.sharedProfile.Add<ChromaticAberration>(false);
            }
        }

        public void Increase(float startValue, float endValue, float increasePerSecond = 0.2f, Action callback = null) {

        }

        public void Set(float startValue, float endValue, float decreasePerSecond = 0.2f, Action callback = null) { // TODO: Current value store issue
            // Tween.ValueTween.To(this, () => {
                
            // }, () => {

            // }, () => {

            // }, callback);
        }
    }
}