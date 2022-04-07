

namespace Objects.Interaction
{  
   public class InteractionManager : Singleton<InteractionManager>
   {
      Interactable CurrentActiveInteraction;

      public void SetInteraction(Interactable interactable)
               => CurrentActiveInteraction = interactable;

      public void UnSetInteraction(Interactable interactable)
      {
         if (CurrentActiveInteraction == interactable)
            CurrentActiveInteraction = null;
      }

      /// <summary>
      /// 상호작용 가능한 오브젝트를 null 로 바꿉니다.
      /// </summary>
      public void ClearInteraction()
            => CurrentActiveInteraction = null;

      /// <summary>
      /// 상호작용 합니다.
      /// </summary>
      public void Interact()
            => CurrentActiveInteraction?.Interact();
   }
}