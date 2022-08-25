using System.Collections.Generic;
using Objects.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
    public class BothInteractableObject : InteractableObjects, IInteractable
    {
        // Collision에서 Normal 벡터를 빼내기 위해서 존재하는 변수
        public Vector3 colNormalVec;

        // 충돌 시의 Velocity
        public Vector3 colVelocity;

        // 충돌 지점
        public Vector3 colPosition;

        public bool canCollision = true;
        public bool canTrigger = false;
        public bool canInteract = true;

        private bool _cancol = true;
        private bool _canTri = false;
        private bool _canInter = true;


        [field: SerializeField]
        public bool CanInteractByPlayer { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _cancol = canCollision;
            _canTri = canTrigger;
            _canInter = canInteract;
        }

        public void Disable()
        {
            canCollision = false;
            canTrigger = false;
            canInteract = false;
        }

        public void Enable()
        {
            canCollision = _cancol;
            canTrigger = _canTri;
            canInteract = _canInter;
        }

        #region Unity Collision Event
        private void OnCollisionEnter(Collision other)
        {
            if (!canCollision) return;

            colNormalVec = other.contacts[0].normal;
            colVelocity = other.relativeVelocity;
            colPosition = other.contacts[0].point;

            OnEventTrigger(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!canTrigger) return;

            OnEventTrigger(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!canCollision || EventIsToggle) return;

            OnEventExit(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!canTrigger || EventIsToggle) return;

            OnEventExit(other.gameObject);
        }
        #endregion // Unity Collision Event

        public void Interact()
        {
            OnEventTrigger(null);
        }
        public void Focus() { }
        public void DeFocus() { }
    }
}