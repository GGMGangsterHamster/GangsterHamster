using UnityEngine;
using TMPro;
using Tween;
using System;

namespace UI.Dialog
{
    public class DialogObject : MonoBehaviour
    {
        const float HALF_PI = Mathf.PI / 2.0f;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private RectTransform _background;
        [SerializeField] private CanvasGroup _cvsGroup;
        public Vector2 padding = new Vector2(20, 10);

        public float fadeinDuration  = 0.5f;
        public float fadeoutDuration = 0.8f;

        private RectTransform _trm;
        private Vector2 _target;
        private Coroutine _movingRoutine;

        private void Awake()
        {
            _trm = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 다이얼로그를 띄움
        /// </summary>
        /// <param name="text">다이얼로그 텍스트</param>
        public void Enable(string text)
        {
            float step = HALF_PI / fadeinDuration;
            float t = 0.0f;

            _text.text = text;
            _background.sizeDelta = _text.GetPreferredValues() + padding;
            _cvsGroup.alpha = 0.0f;

            gameObject.SetActive(true);

            ValueTween.To(this, () => {
                t += step * Time.deltaTime;
                _cvsGroup.alpha = Mathf.Sin(t);
            }, () => t >= HALF_PI, () => {
                _cvsGroup.alpha = 1.0f;
            });
        }

        /// <summary>
        /// 다이얼로그를 비활성화함
        /// </summary>
        public void Disable(Action callback = null)
        {
            float step = HALF_PI / fadeoutDuration;
            float t = 0.0f;

            ValueTween.To(this, () => {
                t += step * Time.deltaTime;
                _cvsGroup.alpha = 1.0f - Mathf.Sin(t);
            }, () => t >= HALF_PI, () => {
                _cvsGroup.alpha = 0.0f;
                callback?.Invoke();

                gameObject.SetActive(false);
            });
        }

        public void Move(float y, float duration)
        {
            if (_movingRoutine != null)
            {
                ValueTween.Stop(this, _movingRoutine);
                _trm.anchoredPosition = _target;
            }

            Vector2 pos = _trm.anchoredPosition;
            _target = _trm.anchoredPosition;
            _target.y -= y;

            float defaultYPos = pos.y;
            float step = duration / (HALF_PI / y);
            float t = 0.0f;

            _movingRoutine = ValueTween.To(this, () => {
                t += step * Time.deltaTime;
                pos.y = defaultYPos - Mathf.Sin(t) * y;
                _trm.anchoredPosition = pos;
            }, () => t >= HALF_PI, () => {
                _trm.anchoredPosition = _target;
                _movingRoutine = null;
            });
        }
    }
}