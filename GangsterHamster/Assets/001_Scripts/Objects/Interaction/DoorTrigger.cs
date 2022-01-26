using System;
using UnityEngine;

namespace Objects.Interactable
{
    public class DoorTrigger : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private Door doorToInteract;
        [SerializeField] private bool _isOpen = false;


        private void OnCollisionEnter(Collision other)
        {
            Collision(other.gameObject);
        }

        /// <summary>
        /// 무기와 충돌했을 시 호출
        /// </summary>
        public void Collision(GameObject collision, Action callback = null)
        {
            if(collision.CompareTag("WEAPON")) {
                Interact();
            }
        }


        public void Interact(Action callback = null)
        {
            switch(_isOpen)
            {
                case false: doorToInteract.Interact(() => { _isOpen = true; }); break;
                case true: { doorToInteract.Release(); _isOpen = false; break; }
            }
        }

        public void Initialize(Action callback = null) { }
        public void Release() { }
    }
}