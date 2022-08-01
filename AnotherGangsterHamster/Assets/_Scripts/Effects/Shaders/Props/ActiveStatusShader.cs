using Objects;
using Tween;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Effects.Shaders.Props
{
   public class ActiveStatusShader : MonoBehaviour, IEventable
   {
      const string EMISSION = "_Emission";
      const string TRANSITION = "_Transition";

      public string targetName = "cain";

      public float duration = 1.0f;
      public float emission = 1.0f;
      public bool defaultStatus = false;

      private List<Material> _mat = new List<Material>();
      private Coroutine _curTween;

      private float _transition;

      private void Awake()
      {
         Renderer[] renderers = GetComponentsInChildren<Renderer>();
         for (int i = 0; i < renderers.Length; ++i)
         {
            if (renderers[i].name.Contains(targetName))
               _mat.Add(renderers[i].material);
         }

         _mat.ForEach(e => {
            e.SetFloat(EMISSION, emission);
            e.SetFloat(TRANSITION, (defaultStatus ? 1.0f : 0.0f));
         });
      }

      public void Active(GameObject other)
      {
         float step = 1.0f / duration;

         if (_curTween != null) {
            ValueTween.Stop(this, _curTween);
         }

         _curTween = ValueTween.To(this, () => {
            _transition += step * Time.deltaTime;
            _mat.ForEach(e => e.SetFloat(TRANSITION, _transition));

         }, () => _transition >= 1.0f, () => {

            _transition = 1.0f;
            _mat.ForEach(e => e.SetFloat(TRANSITION, 1.0f));
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
            _mat.ForEach(e => e.SetFloat(TRANSITION, _transition));

         }, () => _transition <= 0.0f, () => {

            _transition = 0.0f;
            _mat.ForEach(e => e.SetFloat(TRANSITION, 0.0f));
            _curTween = null;
         });
      }
      
   }
}