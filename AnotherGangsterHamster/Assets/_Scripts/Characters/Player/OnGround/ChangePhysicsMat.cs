using Objects;
using UnityEngine;

namespace Characters.Player.OnGround
{  
   public class ChangePhysicsMat : MonoBehaviour, IEventable
   {
      [Header("Transform of Player\n(if null, root transform will be used)")]
      [Space(0.5f)]
      public Transform Player = null;
      
      private GameObject _curStandingObj = null; // Deactive 에서 Collider GetComponent 매번 하는 것 대안으로 사용
      private Collider _curStandingCollider = null;

      private int _groundLayer = LayerMask.NameToLayer("GROUND");

      private void Awake()
      {
         if (Player == null)
            Player = transform.root;
      }

      public void Active(GameObject other)
      {
         Collider collider = other.GetComponent<Collider>();
         if (collider == null || _groundLayer == other.layer) return;

         float angle =
            Vector3.Angle(Player.position,
            collider.ClosestPointOnBounds(Player.position));

         //Debug.Log(angle);
         
         if (angle <= 10.0f) // 임시 값
         {
            if (_curStandingObj != null)
               Deactive(_curStandingObj);

            _curStandingObj      = other;
            _curStandingCollider = collider;
            collider.material.dynamicFriction = 10.0f;
            collider.material.staticFriction  = 10.0f;
         }
      }

      public void Deactive(GameObject other)
      {
         if (other != null && other == _curStandingObj)
         {
            _curStandingCollider.material.dynamicFriction = 0.0f;
            _curStandingCollider.material.staticFriction  = 0.0f;
            _curStandingCollider = null;
            _curStandingObj      = null;
         }
      }
   }
}