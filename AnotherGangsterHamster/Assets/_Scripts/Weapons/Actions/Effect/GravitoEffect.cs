using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


namespace Weapons.Actions.Effect
{
    public class GravitoEffect : MonoBehaviour
    {
        public Volume globalVolume;

        private MotionBlur motionBlur;

        private void Awake()
        {
            if(!globalVolume.profile.TryGet(out motionBlur))
            {
                Debug.Log("왜 없어요?");
            }
            else
            {
                motionBlur.active = true;
            }
            globalVolume.enabled = false;
        }

        public void EffectOn()
        {
            Debug.Log("asdf");
            globalVolume.enabled = true;
        }

        public void EffectOff()
        {
            Debug.Log("fads");
            globalVolume.enabled = false;
        }

        private void OnDisable()
        {
            globalVolume.enabled = false;
        }
    }
}

