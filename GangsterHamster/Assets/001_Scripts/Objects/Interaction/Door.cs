using System;
using Objects.Interactable.Management;
using Objects.UI.Management;
using UnityEngine;

namespace Objects.Interactable
{

    public class Door : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private GameObject _doorObject;
        [SerializeField] private float _defaultDoorStayOpenDuration = 3.0f;
        [SerializeField] private Transform[] _uiPositions = new Transform[2];


        /// <summary>
        /// 문을 엽니다<br/>
        /// 알아서 Release 해 주니 호출할 필요 없음
        /// </summary>
        public void Interact(Action callback = null)
        {
            _doorObject.SetActive(false);
            Invoke(nameof(Release), _defaultDoorStayOpenDuration); // 오 이런
            FloatingUIManager.Instance.DisableUI();
            

            callback?.Invoke();
        }

        public void Release() // 문을 닫음
        {
            _doorObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PLAYER_BASE")) {
                FloatingUIManager.Instance.KeyHelper(KeyCode.E, "를 눌러 문을 여세요.", GameManager.Instance.FindClosestPosition(_uiPositions));
                InteractionManager.Instance.SetInteraction(() => {
                    Interact();
                });
            }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.CompareTag("PLAYER_BASE")) {
                FloatingUIManager.Instance.DisableUI();
                InteractionManager.Instance.UnSetInteraction(() => {
                    Interact();
                });
            }
        }

        public void Initialize(Action callback = null) { }
        public void Collision(GameObject collision, Action callback = null) { }
    }
}
