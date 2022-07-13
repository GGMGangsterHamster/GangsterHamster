using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Weapons.Actions.Effect
{
    public class LumoCubeEffect : MonoBehaviour
    {
        public Volume globalVolume;
        public float maxAberationIntensity = 0.5f;

        private ChromaticAberration aberration;
        private bool EffectiveActivate = false;
        private float currentTime = 0f;

        private void Awake()
        {
            globalVolume.enabled = false;
            globalVolume.profile.TryGet(out aberration);
        }

        public void EffectOn()
        {
            globalVolume.enabled = true;

            EffectiveActivate = true;
        }

        public void EffectOff()
        {
            EffectiveActivate = false;
        }

        private void Update()
        {
            if(EffectiveActivate && currentTime < maxAberationIntensity)
            {
                currentTime += Time.deltaTime * 7;
                aberration.intensity.Override(currentTime);
            }
            else if(!EffectiveActivate && currentTime > 0f)
            {
                currentTime -= Time.deltaTime * 7;
                aberration.intensity.Override(currentTime);

                if (currentTime <= 0f)
                {
                    globalVolume.enabled = false;
                }
            }
        }

        private void OnDisable()
        {
            globalVolume.enabled = false;
            currentTime = 0f;
        }
    }
}
