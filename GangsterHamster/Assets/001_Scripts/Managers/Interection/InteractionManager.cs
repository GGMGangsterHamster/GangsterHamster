using System;
using UnityEngine;

namespace Objects.Interactable.Management
{
    public class InteractionManager : MonoSingleton<InteractionManager>
    {
        /// <summary>
        /// 현제 상호작용이 가능한 오브젝트
        /// </summary>
        private Action CurrentActiveInteraction;

        /// <summary>
        /// 상호작용 할 수 있게 합니다.
        /// </summary>
        /// <param name="action">상호작용 시 호출됨</param>
        public void SetInteraction(Action action)
        {
            CurrentActiveInteraction = action;
        }

        /// <summary>
        /// 상호용 할 수 없게 합니다.
        /// </summary>
        /// <param name="action">상호작용 시 호출된다고 넣었던 거</param>
        public void UnSetInteraction(Action action)
        {
            if(CurrentActiveInteraction == action) {
                CurrentActiveInteraction = null;
            }
        }

        /// <summary>
        /// 상호작용 합니다.
        /// </summary>
        public void Interact()
        {
            CurrentActiveInteraction?.Invoke();
        }
        
    }
}