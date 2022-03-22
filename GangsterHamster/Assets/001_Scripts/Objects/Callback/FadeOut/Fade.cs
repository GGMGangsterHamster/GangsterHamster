using UnityEngine;
using UnityEngine.UI;
using Tween;

namespace Objects.Callback
{
   public class Fade : MonoBehaviour, ICallbackable
   {
      [SerializeField] private CanvasGroup   cvs;
      [SerializeField] private float         fadeDuration = 1.0f;
      
      [Range(0.0f, 1.0f)]
      [SerializeField] private float targetAlpha = 1.0f;


      private void Awake()
      {
         cvs.alpha = 0.0f;
      }

      public void Invoke(object param)
      {
         float step = targetAlpha / fadeDuration;
         cvs.alpha = 0.0f;

         int counter = 0;

         ValueTween.To(this, () => {
            cvs.alpha += step * Time.deltaTime;
            ++counter;
         }, () => {
            return cvs.alpha >= 1 || counter > 10000;
         }, () => {
            ExecuteCallback.Call(this.transform);
         });

         Debug.Log(counter);
      }
   }
}