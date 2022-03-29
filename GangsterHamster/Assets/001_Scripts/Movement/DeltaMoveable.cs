using UnityEngine;

namespace Movement.Delta
{
   public class DeltaMoveable : MonoBehaviour
   {
      private Vector2 _delta;
      private Vector3 _rawDelta;

      public DeltaMoveable(Vector2 initalDelta)
      {
         _delta = initalDelta;
         _rawDelta = Vector3.zero;
      }

      #region Set

      public void SetRawDelta(Vector3 rawDelta)
      {
         _rawDelta = rawDelta;
      }

      public void AddRawDelta(Vector3 rawDelta)
      {
         _rawDelta += rawDelta;
      }

      /// <summary>
      /// Delta 를 설정합니다.
      /// </summary>
      public void SetDelta(Vector2 delta)
      {
         _delta = delta;
      }

      /// <summary>
      /// Delta 를 설정합니다.
      /// </summary>
      public void SetDelta(float x, float y)
      {
         _delta.x = x;
         _delta.y = y;
      }


      /// <summary>
      /// Delta 의 x 를 설정합니다.
      /// </summary>
      public void SetXDelta(float x)
      {
         _delta.x = x;
      }

      /// <summary>
      /// Delta 의 x 를 설정합니다.
      /// </summary>
      public void SetYDelta(float y)
      {
         _delta.y = y;
      }

#endregion // Set

      #region Add

      /// <summary>
      /// Delta 를 더합니다.
      /// </summary>
      public void AddDelta(Vector2 delta)
      {
         _delta += delta;
      }

      /// <summary>
      /// Delta 를 더합니다.
      /// </summary>
      public void AddDelta(float x, float y)
      {
         _delta.x += x;
         _delta.y += y;
      }

      /// <summary>
      /// Delta 의 x 를 더합니다
      /// </summary>
      public void AddXDelta(float x)
      {
         _delta.x += x;
      }

      /// <summary>
      /// Delta 의 y 를 더합니다
      /// </summary>
      public void AddYDelta(float y)
      {
         _delta.y += y;
      }

      #endregion // Add

      /// <summary>
      /// Delta 를 0 으로 초기화합니다.
      /// </summary>
      public void ResetDelta()
      {
         _delta.x = 0.0f;
         _delta.y = 0.0f;
      }

      /// <summary>
      /// Delta 의 magnitude 를 1 로 clamp 한 결과를 반환합니다.
      /// </summary>
      public Vector2 ClampDelta()
      {
         return _delta.normalized;
      }

      /// <summary>
      /// 플레이어가 이동하는 방향을 가져옵니다.
      /// </summary>
      public Vector2 GetDelta()
      {
         return _delta;
      }

      /// <summary>
      /// 이 스크립트가 부착된 오브젝트의 회전에 따른 delta 를 계산해 반환합니다.
      /// </summary>
      public Vector3 Calculate()
      {
         Vector3 delta = _delta;
         delta.z = delta.y;
         delta.y = 0.0f;

         Vector3 finalDelta =
            (transform.TransformDirection(delta) + _rawDelta) * Time.deltaTime;
            
         _rawDelta = Vector3.zero;
         return finalDelta;
      }
   }
}