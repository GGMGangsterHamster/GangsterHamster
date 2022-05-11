using System;
using UnityEngine;

namespace Objects.Interactable.Management
{
    public class InteractionManager : Singleton<InteractionManager>, ISingletonObject
    {
        /// <summary>
        /// 현제 상호작용이 가능한 오브젝트
        /// </summary>
        private Interactable CurrentActiveInteraction;

        /// <summary>
        /// 상호작용 할 수 있게 합니다.
        /// </summary>
        public void SetInteraction(Interactable interactable)
        {
            CurrentActiveInteraction = interactable;
        }

        /// <summary>
        /// 상호작용 할 수 없게 합니다.
        /// </summary>
        public void UnSetInteraction(Interactable interactable)
        {
            if(CurrentActiveInteraction == interactable) {
                CurrentActiveInteraction = null;
            }
        }

        /// <summary>
        /// 상호작용 불가능한 상태로 변경합니다.
        /// </summary>
        public void ClearInteraction()
        {
            CurrentActiveInteraction = null;
        }

        /// <summary>
        /// 상호작용 합니다.
        /// </summary>
        public void Interact()
        {
            CurrentActiveInteraction?.Interact();
        }
        
    }
}