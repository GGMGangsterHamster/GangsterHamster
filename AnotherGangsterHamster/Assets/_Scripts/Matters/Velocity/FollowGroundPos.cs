using Characters.Player;
using Characters.Player.Move;
using Objects;
using UnityEngine;

namespace Matters.Velocity
{
   [RequireComponent(typeof(MoveDelta),
                     typeof(CollisionInteractableObject))]
   public class FollowGroundPos : MonoBehaviour, ICollisionEventable
   {
      private CollisionInteractableObject _colInterObj;
      private MoveDelta _delta;

      // FIXME: change to private
      public Transform _curStandingPos;
      private Vector3 _pastPosition = Vector3.zero;

      private void Awake()
      {
         _colInterObj
            = GetComponent<CollisionInteractableObject>();

         _delta = GetComponent<MoveDelta>();
      }

      public void Active(GameObject other)
      {
         _curStandingPos   = other.transform;
         _pastPosition     = _curStandingPos.localPosition;
      }

      public void Deactive(GameObject other)
      {
         if (_curStandingPos == other.transform)
            _curStandingPos = null;
      }

      private void FixedUpdate()
      {

         if (_curStandingPos != null)
         {
            // Debug.Log("CUR: " + _curStandingPos.localPosition);
            // Vector3 d = _curStandingPos.localPosition - _pastPosition;
            // Debug.Log("D  : " + d);
            // _delta.AddRawDelta(d);
         }

         // transform.Translate(_delta.Calculate(transform, PlayerValues.Speed, true));
      }
   }
}