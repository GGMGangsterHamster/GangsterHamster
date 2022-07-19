using UnityEngine;

namespace Objects.Interaction
{
   public interface IInteractable
   {
      // 플레이어가 직접 상호작용 가능한 오브젝트인지
      public bool CanInteractByPlayer { get; }
      public bool Activated { get; }

      /// <summary>
      /// 상호 작용 시 호출
      /// </summary>
      public void Interact();

      /// <summary>
      /// 포커스 상태 시 호출
      /// </summary>
      public void Focus();

      /// <summary>
      /// 포커스 상태 벗어날 때 호출
      /// </summary>
      public void DeFocus();
      
   }
}