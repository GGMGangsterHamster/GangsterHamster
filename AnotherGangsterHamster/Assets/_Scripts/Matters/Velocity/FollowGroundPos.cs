using Characters.Player;
using Characters.Player.Move;
using Objects;
using Objects.StageObjects;
using UnityEngine;

namespace Matters.Velocity
{
    // [RequireComponent(typeof(MoveDelta))]
    public class FollowGroundPos : MonoBehaviour, IEventable
    {
        public bool Enabled { get; set; } = true;

        private FollowGroundPosOption _curGround = null;

        private Transform _curRootTrm;
        private Vector3 _pastPos;

        public void Active(GameObject other)
        {
            if (!Enabled) return;

            FollowGroundPosOption newGround
                = other?.GetComponent<FollowGroundPosOption>();
            int priority = newGround?.priority ?? 0;
            int curPriority = _curGround?.priority ?? 0;

            if (this.gameObject.name == "GroundChecker")
                Debug.Log(other?.name);

            if (priority >= curPriority)
            {
                _curGround = newGround;
                transform.SetParent(other?.transform);
            }
        }

        public void Deactive(GameObject other)
        {
            if (!Enabled) return;

            bool avaliable = true;

            if (_curGround != null)
            {
                FollowGroundPosOption newGround
                    = other?.GetComponent<FollowGroundPosOption>();
                int priority = newGround?.priority ?? 0;
                int curPriority = _curGround.priority;

                avaliable = priority >= curPriority;
            }
            
            if (avaliable)
            {
                _curGround = null;
                transform.SetParent(null);
            }
        }
    }
}