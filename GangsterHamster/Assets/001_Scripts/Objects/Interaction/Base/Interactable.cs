using UnityEngine;
using System;

namespace Objects.Interactable
{
    /// <summary>
    /// 상호 작용 가능한 오브젝트가 상속받아야 하는 부모 클레스
    /// </summary>
    abstract public class Interactable : MonoBehaviour
    {
        /// <summary>
        /// 상호 작용 용도로 호출되어야 함
        /// </summary>
        /// <param name="callback">callback (if needed)</param>
        abstract public void Interact(Action callback = null);

        /// <summary>
        /// 포커스 상태일 때
        /// </summary>
        abstract public void Focus(Action callback = null);

        /// <summary>
        /// 포커스 상태 벗어날 때
        /// </summary>
        abstract public void DeFocus(Action callback = null);

        /// <summary>
        /// 사용하고 난 후 호출해야함
        /// </summary>
        abstract public void Release();

        /// <summary>
        /// 충돌 용도로 호출되어야 함
        /// </summary>
        /// <param name="collision">본인의 GameObject</param>
        /// <param name="callback">callback (if needed)</param>
        abstract public void Collision(UnityEngine.GameObject collision, Action callback = null); // 여기 두기 애매하긴 함
    }
}