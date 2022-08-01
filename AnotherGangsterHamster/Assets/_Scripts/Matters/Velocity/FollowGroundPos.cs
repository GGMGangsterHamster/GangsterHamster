using Characters.Player;
using Characters.Player.Move;
using Objects;
using Objects.StageObjects;
using UnityEngine;

namespace Matters.Velocity
{
   [RequireComponent(typeof(MoveDelta))]
   public class FollowGroundPos : MonoBehaviour, IEventable
   {
      private MoveDelta _delta;

      private Transform _curRootTrm;
      private Vector3 _pastPos;

      public bool Calculate { get; set; } = true;

      private void Awake()
      {
         _delta = GetComponent<MoveDelta>();
      }

      public void Active(GameObject other)
      {
         _curRootTrm = other?.transform;

         if(_curRootTrm != null)
         {
            _pastPos = other.transform.position;
         }
      }
      
      public void Deactive(GameObject other)
      {
         if (_curRootTrm == other.transform)
         {
            _curRootTrm = null;
         }
      }

      private void FixedUpdate()
      {
         if (_curRootTrm == null || !Calculate) return;

         Vector3 d = _curRootTrm.position - _pastPos;
         _delta.AddRawDelta(d);
         _pastPos = _curRootTrm.position;
         
      }
   }
}