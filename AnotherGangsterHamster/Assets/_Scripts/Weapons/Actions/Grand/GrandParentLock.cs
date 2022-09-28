using UnityEngine;
using Matters.Velocity;
using Objects;

namespace Weapons.Actions
{
    [RequireComponent(typeof(FollowGroundPos))]
    public class GrandParentLock : MonoBehaviour, IEventable
    {
        private FollowGroundPos _setParent;

        private void Awake()
        {
            _setParent = GetComponent<FollowGroundPos>();
        }

        public void Active(GameObject other)
        {
            _setParent.Enabled = true;
        }

        public void Deactive(GameObject other)
        {
            _setParent.Enabled = false;
        }
    }
}