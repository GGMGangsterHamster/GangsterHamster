using Tween;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace PostProcessing
{
   /// <summary>
   /// Volume 을 사용하는 효과가 상속받는 클레스
   /// </summary>
   abstract public class Effects<GlobalVolume> : MonoBehaviour
            where GlobalVolume : VolumeComponent
   {
      protected Volume _volume;
      protected GlobalVolume _globalVolume;

      // 코루틴 저장 용
      private Coroutine _coroutine = null;

      private void Awake()
      {
         _volume = GetComponent<Volume>();

#if UNITY_EDITOR
         NULL.Check(_volume, () => this.enabled = false);
#endif
         if (!_volume.profile.TryGet<GlobalVolume>(out var globalVolume))
            _globalVolume = _volume.profile.Add<GlobalVolume>(false);
         else  // 이렇게 안하면 안 들어가져서
            _globalVolume = globalVolume;
      }

      /// <summary>
      /// Tween 합니다.
      /// </summary>
      /// <param name="initalize">초기화</param>
      /// <param name="step">단계</param>
      /// <param name="compare">비교</param>
      protected void Tween(Action initalize,
                           Action step,
                           Func<bool> compare,
                           Action callback = null)
      {

         if (_coroutine != null)
         { // 이미 재생중인 경우
            ValueTween.Stop(this, _coroutine);
         }

         initalize();

         callback += () =>
         { // 효과 종료 처리
            _coroutine = null;
         };

         _coroutine = ValueTween.To(this, step, compare, callback);
      }

      /// <summary>
      /// Global 로 사용되는 volume 을 반환합니다.
      /// </summary>
      public GlobalVolume GetGlobalVolume()
      {
         return _globalVolume;
      }

   }
}