using Objects;
using Tween;
using UnityEngine;

namespace Effects.Shaders.Props
{
   public class ActiveStatusShader : MonoBehaviour, ICollisionEventable
   {
      const string EMISSION = "_Emission";
      const string TRANSITION = "_Transition";

      public float duration = 1.0f;
      public float emission = 1.0f;
      public bool defaultStatus = false;

      private Material _mat;
      private Coroutine _curTween;

      private float _transition;

      private void Awake()
      {
         _mat = GetComponent<Renderer>().sharedMaterial;
         _mat.SetFloat(EMISSION, emission);
         _mat.SetFloat(TRANSITION, (defaultStatus ? 1.0f : 0.0f));
      }

      public void Active(GameObject other)
      {
         float step = 1.0f / duration;

         if (_curTween != null) {
            ValueTween.Stop(this, _curTween);
         }

         _curTween = ValueTween.To(this, () => {
            _transition += step * Time.deltaTime;
            _mat.SetFloat(TRANSITION, _transition);

         }, () => _transition >= 1.0f, () => {

            _transition = 1.0f;
            _mat.SetFloat(TRANSITION, 1.0f);
            _curTween = null;
         });
      }

      public void Deactive(GameObject other)
      {
         float step = 1.0f / duration;

         if (_curTween != null) {
            ValueTween.Stop(this, _curTween);
         }

         _curTween = ValueTween.To(this, () => {
            _transition -= step * Time.deltaTime;
            _mat.SetFloat(TRANSITION, _transition);

         }, () => _transition <= 0.0f, () => {

            _transition = 0.0f;
            _mat.SetFloat(TRANSITION, 0.0f);
            _curTween = null;
         });
      }
      
   }
}