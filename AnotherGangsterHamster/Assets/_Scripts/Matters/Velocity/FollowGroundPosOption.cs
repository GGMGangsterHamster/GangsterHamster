using UnityEngine;

namespace Matters.Velocity
{
    
    public class FollowGroundPosOption : MonoBehaviour
    {
        public int priority = 0;
        public bool doNotFollow { get; set; } = false;
    }
}