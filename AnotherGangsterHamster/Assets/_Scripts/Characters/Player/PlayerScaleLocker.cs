using Matters.Velocity;
using Objects;
using UnityEngine;
using Weapons.Actions.Broker;

namespace Characters.Player
{
    [RequireComponent(typeof(FollowGroundPos))]
    public class PlayerScaleLocker : MonoBehaviour, IEventable
    {
        private const string GRAND = "Grand";
        private FollowGroundPos _groundFollower;

        private void Start()
        {
            GrandMessageBroker grandEvent = FindObjectOfType<GrandMessageBroker>();
            _groundFollower = GetComponent<FollowGroundPos>();
            Debug.Assert(grandEvent != null);
            Debug.Assert(_groundFollower != null);

            grandEvent.OnUse.AddListener(a => {
                Active(null);
            });

            grandEvent.OnChangedEnd.AddListener(() => {
                Deactive(null);
            });
        }


        public void Active(GameObject other)
        {
            if (DoesParentContainsGrand(this.transform))
            {
                this.transform.SetParent(null);
                _groundFollower.Enabled = false;
            }
        }

        public void Deactive(GameObject other)
        {
            _groundFollower.Enabled = true;
        }


        private bool DoesParentContainsGrand(Transform trm)
        {
            if (trm.parent == null)
                return false;

            return (trm.parent.name.CompareTo(GRAND) == 0)
                    ? true : DoesParentContainsGrand(trm.parent);
        }
    }
}