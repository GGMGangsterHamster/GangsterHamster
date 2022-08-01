using Objects;
using Objects.InteractableObjects;
using Tween;
using UnityEngine;

namespace Effects.Shaders.TypeObject.Hexagon
{
   [RequireComponent(typeof(ATypeHexagon), typeof(TriggerInteractableObject))]
   public class HexagonBrightnessEffect : MonoBehaviour, IEventable
   {
      public float _maxAlpha = 1.0f;
      public float _minAlpha = 0.0f;
      public float _defMaxAlpha = 0.25f;

      public float collisionAlphaEffectDuration = 1.0f;

      private float _curAlpha;
      private Coroutine _curRoutine;
      private Coroutine _maxAlphaRoutine;
      private Material _mat;

      private void Awake()
      {
         _mat = GetComponent<Renderer>().material;
      }

      public void Active(GameObject other)
      {
         float curMaxAlpha = (_curAlpha == 0 ? _defMaxAlpha : _curAlpha);
         float step = 1.0f / collisionAlphaEffectDuration;

         if (_curRoutine != null)
            ValueTween.Stop(this, _curRoutine);

         if (_curRoutine != null)
            ValueTween.Stop(this, _maxAlphaRoutine);

         _curRoutine = ValueTween.To(this, () => {
            _curAlpha += step * Time.deltaTime;
            _mat.SetFloat(ATypeHexagon.MINALPHA, _curAlpha);
         }, () => _curAlpha >= _maxAlpha, () => {
            _curAlpha = _maxAlpha;
            _mat.SetFloat(ATypeHexagon.MINALPHA, _maxAlpha);
            _curRoutine = null;
         });

         _maxAlphaRoutine = ValueTween.To(this, () => {
            curMaxAlpha += step * Time.deltaTime;
            _mat.SetFloat(ATypeHexagon.MAXALPHA, curMaxAlpha);
         }, () => curMaxAlpha >= _maxAlpha, () => {
            _mat.SetFloat(ATypeHexagon.MAXALPHA, _maxAlpha);
            _maxAlphaRoutine = null;
         });
      }

      public void Deactive(GameObject other)
      {
         float curMaxAlpha = _curAlpha;
         float step = 1.0f / collisionAlphaEffectDuration;

         if (_curRoutine != null)
            ValueTween.Stop(this, _curRoutine);

         if (_curRoutine != null)
            ValueTween.Stop(this, _maxAlphaRoutine);

         _curRoutine = ValueTween.To(this, () => {
            _curAlpha -= step * Time.deltaTime;
            _mat.SetFloat(ATypeHexagon.MINALPHA, _curAlpha);
         }, () => _curAlpha <= _minAlpha, () => {
            _curAlpha = _minAlpha;
            _mat.SetFloat(ATypeHexagon.MINALPHA, _minAlpha);
            _curRoutine = null;
         });

         _maxAlphaRoutine = ValueTween.To(this, () => {
            curMaxAlpha -= step * Time.deltaTime;
            _mat.SetFloat(ATypeHexagon.MAXALPHA, curMaxAlpha);
         }, () => curMaxAlpha <= _defMaxAlpha, () => {
            _mat.SetFloat(ATypeHexagon.MAXALPHA, _defMaxAlpha);
            _maxAlphaRoutine = null;
         });
      }
   }
}