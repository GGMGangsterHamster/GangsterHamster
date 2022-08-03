using Effects.Fullscreen;
using Stages.Management;
using UnityEngine;

namespace Characters.Player
{
    public class OnDeadEvent : MonoBehaviour
    {

        private Fade  _fade;
        public Fade @Fade {
            get
            {
                if (_fade == null)
                    _fade = FindObjectOfType<Fade>();
                return _fade;
            }
        }

        public float fadeoutDuration = 2.0f;

        public void Execute()
        {
            @Fade.FadeOut(fadeoutDuration, () => {
                StageManager.Instance.Reload();
            });
        }
    }
}