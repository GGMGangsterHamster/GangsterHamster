using UnityEngine;
using UnityEngine.UI;
using Tween;

namespace Objects.Callback
{
   public class Fade : MonoBehaviour, ICallbackable
   {
      [SerializeField] private CanvasGroup   cvs;
      [SerializeField] private float         fadeDuration = 1.0f;
      [SerializeField] private bool          activeWhenStart = false;

      [Range(0.0f, 1.0f)]
      [SerializeField] private float defaultAlpha = 0.0f;
      [Range(0.0f, 1.0f)]
      [SerializeField] private float targetAlpha = 1.0f;


      private void Awake()
      {
         cvs.alpha = defaultAlpha;

         if (targetAlpha <= 0.0f)
         {
            targetAlpha = -1.0f;
         }
      }

      private void Start()
      {
         if (activeWhenStart)
         {
            Invoke(null);
         }
      }

      public void Invoke(object param)
      {
         float step = targetAlpha / fadeDuration;
         cvs.alpha = defaultAlpha;

         int counter = 0;

         ValueTween.To(this, () => {
            cvs.alpha += step * Time.deltaTime;
            ++counter;
         }, () => {
            return cvs.alpha == targetAlpha;
         }, () => {
            cvs.alpha = targetAlpha;
            ExecuteCallback.Call(this.transform);
         });

         Debug.Log(counter);
      }
   }
}