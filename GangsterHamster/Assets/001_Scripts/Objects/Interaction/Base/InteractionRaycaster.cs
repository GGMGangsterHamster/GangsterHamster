using UnityEngine;
using Player;
using Objects.Interactable.Management;

namespace Objects.Interactable
{
    public class InteractionRaycaster : MonoSingleton<InteractionRaycaster>
    {
        Interactable _currentInteractable = null;


        private void FixedUpdate()
        {
            Interactable target = FireRay();

            if(target == null || !target.canInteractByPlayer) { // 상호작용 가능한 오브젝트 없음
                InteractionManager.Instance.ClearInteraction();
                _currentInteractable?.DeFocus();
                _currentInteractable = null;
                return;
            }

            // 같은 오브젝트인 경우
            if (target == _currentInteractable)
                return;
            
            // 예전 오브젝트 선택 해제
            if(_currentInteractable != null) {
                _currentInteractable.DeFocus();
                InteractionManager.Instance.UnSetInteraction(_currentInteractable);
            }

            // 새 오브젝트 선택
            _currentInteractable = target;
            _currentInteractable.Focus();
            InteractionManager.Instance.SetInteraction(_currentInteractable);
        }

        /// <summary>
        /// 카메라 중심에서 Ray 를 발사함
        /// </summary>
        /// <returns>null when none</returns>
        private Interactable FireRay()
        {
            Transform target = null;
            if(Physics.Raycast(transform.position,
                               transform.TransformDirection(Vector3.forward),
                               out RaycastHit hit,
                               PlayerValues.InteractionMaxDistance)) {
                target = hit.transform;
            }

            return target?.GetComponent<Interactable>();
        }


    }
}