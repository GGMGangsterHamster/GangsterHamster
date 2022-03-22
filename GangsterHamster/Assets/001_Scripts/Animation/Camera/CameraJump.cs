using UnityEngine;
using Tween;
using Player.Movement;
using System.Collections;

namespace Animation.Camera
{  
   public class CameraJump : MonoBehaviour
   {
      readonly Vector3 DEFAULT_POSITION = Vector3.zero;
      const float DOUBLE_PI = Mathf.PI * 2.0f;

      [Header("도움닫기 시간")]
      [SerializeField] private float _preJumpDuration = 0.1f;

      [Header("도움닫기 하강 높이")]
      [SerializeField] private float _preJumpDepth = 0.1f;

      private Coroutine jump;
      private Coroutine land; 

      private void Start()
      {
         PlayerMovement playerMove = FindObjectOfType<PlayerMovement>();

         playerMove.OnJump += () => {

            if (jump != null) // TODO: 확인해봐야함
               ValueTween.Stop(this, jump);

            this.transform.localPosition = DEFAULT_POSITION;
            float step = Mathf.PI / _preJumpDuration;
            float time = Mathf.PI;


            ValueTween.To(this, () => {
               Vector3 delta = Vector3.zero;

               delta.y = Mathf.Sin(time) * _preJumpDepth;
               this.transform.localPosition = delta;

               time += step * Time.deltaTime;
               Debug.Log(time);


            }, () => {
               return time >= DOUBLE_PI;
            }, () => {
               this.transform.localPosition = DEFAULT_POSITION;
            });
         };

         playerMove.OnLanded += () => {

            if (land != null)
               ValueTween.Stop(this, jump);

            
         };
      }
   }
}