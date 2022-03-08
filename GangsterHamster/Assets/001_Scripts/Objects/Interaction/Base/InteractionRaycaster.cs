using UnityEngine;
using Player;
using Objects.Interactable.Management;

namespace Objects.Interactable
{
    public class InteractionRaycaster : MonoSingleton<InteractionRaycaster>
    {
        
        private void FixedUpdate()
        {
            Interactable interactable = null;
            if(FireRay()?.TryGetComponent<Interactable>(out interactable) == true) { // 오 이런
                InteractionManager.Instance.SetInteraction(interactable);
            }
        }

        /// <summary>
        /// 카메라 중심에서 Ray 를 발사함
        /// </summary>
        /// <returns>null when none</returns>
        public Transform FireRay()
        {
            Transform target = null;
            if(Physics.Raycast(transform.position,
                               transform.TransformDirection(Vector3.forward),
                               out RaycastHit hit,
                               PlayerValues.InteractionMaxDistance)) {
                target = hit.transform;
            }

            return target;
        }


    }
}