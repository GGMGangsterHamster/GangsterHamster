using UnityEngine;
using UnityEngine.UI;
using Tween;

namespace Effects.Fullscreen
{
   public class Fade : MonoBehaviour
   {
      Image _fader;

      private void Awake()
      {
         _fader = GetComponentInChildren<Image>();
      }

      public void FadeOut(float duration, System.Action callback = null)
      {
         float step = 1.0f / duration;

         ValueTween.To(this, () =>
         {
            Color c = _fader.color;
            c.a += duration * Time.deltaTime;
            _fader.color = c;
         }, () => _fader.color.a >= 1.0f, callback);
      }

      public void FadeIn(float duration, System.Action callback = null)
      {
         float step = 1.0f / duration;

         ValueTween.To(this, () =>
         {
            Color c = _fader.color;
            c.a -= duration * Time.deltaTime;
            _fader.color = c;
         }, () => _fader.color.a <= 0.0f, callback);
      }
   }
}