using Objects;
using Tween;
using UnityEngine;

namespace UI.Dialog
{    
    public class KeyObject : MonoBehaviour, IEventable
    {
        private const float HALF_PI = Mathf.PI / 2.0f;

        private CanvasGroup _cvsGroup;

        public float fadeinDuration = 0.25f;
        public float fadeoutDuration = 0.25f;

        private void Start()
        {
            _cvsGroup = GetComponent<CanvasGroup>();
            _cvsGroup.alpha = 0.0f;
            gameObject.SetActive(false);
        }


        public void Active(GameObject other)
        {
            float step = HALF_PI / fadeinDuration;
            float t = 0.0f;

            _cvsGroup.alpha = 0.0f;

            gameObject.SetActive(true);

            ValueTween.To(this, () => {
                t += step * Time.deltaTime;
                _cvsGroup.alpha = Mathf.Sin(t);
            }, () => t >= HALF_PI, () => {
                _cvsGroup.alpha = 1.0f;
            });
        }

        public void Deactive(GameObject other)
        {
            if (_cvsGroup.alpha <= 0.0f) return;

            float step = HALF_PI / fadeoutDuration;
            float t = 0.0f;

            ValueTween.To(this, () => {
                t += step * Time.deltaTime;
                _cvsGroup.alpha = 1.0f - Mathf.Sin(t);
            }, () => t >= HALF_PI, () => {
                _cvsGroup.alpha = 0.0f;
                gameObject.SetActive(false);
            });
        }


        
    }
}