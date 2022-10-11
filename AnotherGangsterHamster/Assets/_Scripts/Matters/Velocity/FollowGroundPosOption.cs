using UnityEngine;

namespace Matters.Velocity
{
    
    public class FollowGroundPosOption : MonoBehaviour
    {
        public int priority = 0;

        [field: SerializeField]
        public bool doNotFollow { get; set; } = false;
    }
}