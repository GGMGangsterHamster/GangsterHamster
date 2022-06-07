using UnityEngine;
using UnityEngine.UI;
using Tween;

namespace Effects.Fullscreen
{
   public class Fade : MonoBehaviour
   {
      Image _fader;

      Image Fader
      {
          get
          {
              if(_fader == null)
                  _fader = GetComponentInChildren<Image>();

              return _fader;
          }
      }

      public void SetFader(int a)
      {
          Color c = Fader.color;
          c.a = a;
          Fader.color = c;
      }

      public void FadeOut(float duration, System.Action callback = null)
      {
         float step = 1.0f / duration;

         ValueTween.To(this, () =>
         {
            Color c = Fader.color;
            c.a += duration * Time.deltaTime;
            Fader.color = c;
         }, () => Fader.color.a >= 1.0f, callback);
      }

      public void FadeIn(float duration, System.Action callback = null)
      {
         float step = 1.0f / duration;
         Color c = Fader.color;
         c.a = 1.0f;
         Fader.color = c;

         ValueTween.To(this, () =>
         {
            Color c = Fader.color;
            c.a -= duration * Time.deltaTime;
             Fader.color = c;
         }, () => Fader.color.a <= 0.0f, callback);
      }
   }
}