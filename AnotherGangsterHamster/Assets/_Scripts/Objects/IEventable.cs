using UnityEngine;

namespace Objects
{
   public interface IEventable
   {
      /// <summary>
      /// 활성화 시 불러짐
      /// </summary>
      public void Active(GameObject other);

      /// <summary>
      /// 비활성화 시 불러짐
      /// </summary>
      public void Deactive(GameObject other);
   }
}