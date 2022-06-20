using UnityEngine;

namespace Objects.Interaction
{
   abstract public class Interactable : MonoBehaviour
   {
      // 플레이어가 직접 상호작용 가능한 오브젝트인지
      public bool canInteractByPlayer = false;
        
      public bool Activated => _activated;

        
      protected bool _activated = false;
      /// <summary>
      /// 상호 작용 시 호출
      /// </summary>
      abstract public void Interact();

      /// <summary>
      /// 포커스 상태 시 호출
      /// </summary>
      abstract public void Focus();

      /// <summary>
      /// 포커스 상태 벗어날 때 호출
      /// </summary>
      abstract public void DeFocus();
      
   }
}