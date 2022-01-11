using System;

namespace Interactable.Object
{
    /// <summary>
    /// 상호 작용 가능한 오브젝트가 구현해야 함
    /// </summary>
    public interface IInteractableObject
    {
        /// <summary>
        /// 상호 작용 용도로 호출되어야 함
        /// </summary>
        /// <param name="callback">callback (if needed)</param>
        public void Interact(Action callback = null);

        /// <summary>
        /// 충돌 용도로 호출되어야 함
        /// </summary>
        /// <param name="collision">본인의 GameObject</param>
        /// <param name="callback">callback (if needed)</param>
        public void Collision(UnityEngine.GameObject collision, Action callback = null); // 여기 두기 애매하긴 함
    }
}