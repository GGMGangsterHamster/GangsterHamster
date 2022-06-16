using Objects;
using Tween;
using UnityEngine;

namespace Effects.Shaders.TypeObject
{   
   [RequireComponent(typeof(TriggerInteractableObject))]
   public class ATypeHexagon : MonoBehaviour, ICollisionEventable
   {
      private const string WALLHEIGHT = "_WallHeight";
      private const string WALLSCALE = "_WallScale";
      private const string BLOOM = "_Bloom";
      private const string FADEINGSPEED = "_FadingSpeed";
      private const string MAXALPHA = "_MaxAlpha";
      private const string MINALPHA = "_MinAlpha";

      private readonly float _maxAlpha = 1.0f;
      private readonly float _minAlpha = 0.0f;
      private readonly float _defMaxAlpha = 0.25f;

      public float wallScale = 1.0f;
      public float bloom = 1.0f;
      public float collisionAlphaEffectDuration = 1.0f;

      public Vector3Int considerDirection = Vector3Int.down;
      public Transform baseTransform = null;

      private Material _mat;

      private float _height;
      private float _curAlpha;
      private Coroutine _curRoutine;
      private Coroutine _maxAlphaRoutine;


      private void Awake()
      {
         _mat = GetComponent<Renderer>().material;
         _height = 0;

         _height += (considerDirection.x != 0 ?
            Mathf.Abs(transform.position.x - baseTransform.position.x) : 0);

         _height += (considerDirection.y != 0 ?
            Mathf.Abs(transform.position.y - baseTransform.position.y) : 0);

         _height += (considerDirection.z != 0 ?
            Mathf.Abs(transform.position.z - baseTransform.position.z) : 0);

         _mat.SetFloat(WALLHEIGHT, _height / wallScale);
         _mat.SetFloat(WALLSCALE, 1.0f / wallScale);
      }

      private void Update()
      {
         _mat.SetFloat(BLOOM, bloom);
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
            _mat.SetFloat(MINALPHA, _curAlpha);
         }, () => _curAlpha >= _maxAlpha, () => {
            _curAlpha = _maxAlpha;
            _mat.SetFloat(MINALPHA, _maxAlpha);
            _curRoutine = null;
         });

         _maxAlphaRoutine = ValueTween.To(this, () => {
            curMaxAlpha += step * Time.deltaTime;
            _mat.SetFloat(MAXALPHA, curMaxAlpha);
         }, () => curMaxAlpha >= _maxAlpha, () => {
            _mat.SetFloat(MAXALPHA, _maxAlpha);
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
            _mat.SetFloat(MINALPHA, _curAlpha);
         }, () => _curAlpha <= _minAlpha, () => {
            _curAlpha = _minAlpha;
            _mat.SetFloat(MINALPHA, _minAlpha);
            _curRoutine = null;
         });

         _maxAlphaRoutine = ValueTween.To(this, () => {
            curMaxAlpha -= step * Time.deltaTime;
            _mat.SetFloat(MAXALPHA, curMaxAlpha);
         }, () => curMaxAlpha <= _defMaxAlpha, () => {
            _mat.SetFloat(MAXALPHA, _defMaxAlpha);
            _maxAlphaRoutine = null;
         });
      }
   }
}