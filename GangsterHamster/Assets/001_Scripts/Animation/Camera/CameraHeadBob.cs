using System.Collections;
using Player;
using Player.Status;
using UnityEngine;

namespace Animation.Camera
{
   public class CameraHeadBob : MonoBehaviour
   {
      private const float STEP = Mathf.PI * 2.0f; // 한 걸음의 단계

      [Header("카메라 안정 용 바라볼 Z 거리")]
      public float focusZDepth = 15.0f;

      private float time = 0.0f; // 삼각함수 용
      private float next = STEP; // time 의 목표

      private void Awake()
      {
         StartCoroutine(HeadBob());
      }

      private void Update()
      {
         ShakeViewpointCorrection();
      }
      
      /// <summary>
      /// 이동 시 카메라 흔들림 효과
      /// </summary>
      private IEnumerator HeadBob()
      {
         while (true)
         {
            yield return new WaitUntil(() => PlayerStatus.IsMoving);

            while (time <= next && PlayerStatus.HeadBob)
            {
               time += Time.deltaTime * PlayerValues.headBobFrequency;

               float x = Mathf.Sin(time / 2.0f) * PlayerValues.headBobAmplitude;
               float y = Mathf.Cos(time) * PlayerValues.headBobAmplitude;

               transform.localPosition = new Vector3(x, y, 0.0f);
               yield return null;
            }

            next = time + STEP;

            yield return null;
         }
      }

      /// <summary>
      /// // VR 하는거 보면 엄청 흔들리잔음, 그거 보정해주는 함수
      /// </summary>
      private void ShakeViewpointCorrection()
      {
         if(!PlayerStatus.CameraShakeCorrection) return;

         Vector3 target = transform.position;
         target.z += focusZDepth;
         Quaternion.LookRotation(target,
                                 GameManager.Instance.Player.transform.up);
      }


   }
}