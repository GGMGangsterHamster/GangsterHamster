using System;
using UnityEngine;

namespace Objects.Interactable
{
    public class DoorTrigger : Interactable
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
        public override void Collision(GameObject collision, Action callback = null)
        {
            if(collision.CompareTag("WEAPON")) {
                Interact();
            }
        }


        public override void Interact(Action callback = null)
        {
            switch(_isOpen)
            {
                case false: doorToInteract.Interact(() => { _isOpen = true; }); break;
                case true: { doorToInteract.Release(); _isOpen = false; break; }
            }
        }

        public void Initialize(Action callback = null) { }
        public override void Release() { }

      public override void Focus(Action callback = null)
      {
      }

      public override void DeFocus(Action callback = null)
      {
      }
   }
}