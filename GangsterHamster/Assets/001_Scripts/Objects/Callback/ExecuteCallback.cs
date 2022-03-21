using UnityEngine;

namespace Objects.Callback
{
   public static class ExecuteCallback
   {
      /// <summary>
      /// 자식 오브젝트에 있는 Callback 을 호출합니다.
      /// </summary>
      /// <param name="transform">자신의 transform</param>
      /// <param name="param">매개 변수</param>
      public static void Call(Transform transform, object param = null)
      {
         transform.GetComponentInChildren<ICallbackable>()?.Invoke(param);
      }
   }
}