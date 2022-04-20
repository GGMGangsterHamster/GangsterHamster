using UnityEngine;

static partial class Utils
{
   /// <summary>
   /// 자식 오브젝트에 있는 Callback 을 호출합니다.
   /// </summary>
   /// <param name="transform">자신의 transform</param>
   /// <param name="param">매개 변수</param>
   static public void ExecuteCallback(Transform transform, object param = null)
   {
      int i = transform.childCount - 1;

      while(i >= 0)
      {
         transform.GetChild(i--)
                  .GetComponent<ICallbackable>()
                  ?.Invoke(param);
      }
   }
}
