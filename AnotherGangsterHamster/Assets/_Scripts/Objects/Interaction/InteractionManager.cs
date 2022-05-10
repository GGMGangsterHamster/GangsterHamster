using UnityEngine;

namespace Objects.Interaction
{  
   public class InteractionManager : Singleton<InteractionManager>
   {
      Interactable   _currentActiveInteraction;
      Transform      _currentActiveAtype;

      Flag _atype; // true 시 해제 상태
      public bool Grep { get; set; } // 잡기 상태

      public InteractionManager()
      {
         _atype = new Flag(true);
      }

      public void SetActiveAtype(Transform transform)
      {
         if (_atype.Get()) // 소유권 존재 시
         {
            _currentActiveAtype = transform;
         }
      }

      public void UnSetActiveAtype(Transform transform)
      {
         if (_currentActiveAtype == transform)
         {
            _currentActiveAtype = null;
            _atype.Set();
            Grep = false;
         }
      }

      public void ClearActvieAtype()
      {
         if (_atype.Get() || !Grep)
         {
            _currentActiveAtype = null;
            Grep = false;
            _atype.Set();
         }
      }

      public void SetInteraction(Interactable interactable)
               => _currentActiveInteraction = interactable;

      public void UnSetInteraction(Interactable interactable)
      {
         if (_currentActiveInteraction == interactable)
            _currentActiveInteraction = null;
      }

      /// <summary>
      /// 상호작용 가능한 오브젝트를 null 로 바꿉니다.
      /// </summary>
      public void ClearInteraction()
            => _currentActiveInteraction = null;

      /// <summary>
      /// 상호작용 합니다.<br/>
      /// 들기 가능한 오브젝트가 있다면 듭니다.
      /// </summary>
      public void Interact(System.Action<Transform> onPickup = null)
      {
         if (_currentActiveInteraction != null)
         {
            _currentActiveInteraction.Interact();
         }
         if (_currentActiveAtype != null)
         {
            onPickup?.Invoke(_currentActiveAtype);
            Grep = true;
         }
      }

      // 의존성 때문에 이렇게 함
      // 상호작용 메니저에서 값 얻어와서 
      // 부피 계산하고 플레이어 손에 들려주기 싫었음

   }
}