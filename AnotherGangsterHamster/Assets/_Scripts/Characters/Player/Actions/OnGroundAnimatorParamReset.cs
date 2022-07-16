using UnityEngine;

namespace Characters.Players.Actions
{
   public class OnGroundAnimatorParamReset : MonoBehaviour, ICallbackable
   {
      public float landAnimPlayMinVelocity = 5.0f;
      private float _landAnimVel;

      private readonly int ONGROUND
         = Animator.StringToHash("OnGround");
      private readonly int ONGROUNDSKIPLAND
         = Animator.StringToHash("OnGroundSkipLand");

      private Animator _animator;
      private Rigidbody _rigid;



      private void Start()
      {
         _animator = GameObject.FindWithTag("PLAYER").GetComponent<Animator>();
         _rigid = GetComponentInParent<Rigidbody>();

         _landAnimVel = landAnimPlayMinVelocity * landAnimPlayMinVelocity;
      }

      public void Invoke(object param)
      {
         if ((int)param != 1) return;
         
         if (_rigid.velocity.sqrMagnitude >= _landAnimVel)
            _animator.SetTrigger(ONGROUND);
         else
            _animator.SetTrigger(ONGROUNDSKIPLAND);
      }
   }
}