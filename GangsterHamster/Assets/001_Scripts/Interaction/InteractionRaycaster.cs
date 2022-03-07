using UnityEngine;
using Player;

namespace Objects.Interaction
{
    public class InteractionRaycaster : MonoSingleton<InteractionRaycaster>
    {
        
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