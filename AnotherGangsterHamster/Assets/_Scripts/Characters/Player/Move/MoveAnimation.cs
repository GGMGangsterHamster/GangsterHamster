using UnityEngine;

namespace Characters.Player.Move
{   
   public class MoveAnimation : MonoBehaviour
   {
      [Range(0.0f, 1.0f)]
      [Header("Lerp t value")]
      public float t = 0.05f;

      private Animator _animator;
      private MoveInputHandler _moveInputHandler;

      

      // 에니메이터 Param 조절 용
      readonly int X = Animator.StringToHash("X");
      readonly int Y = Animator.StringToHash("Y");
      private float _targetX = 0.0f;
      private float _targetY = 0.0f;
      private float _curX = 0.0f;
      private float _curY = 0.0f;

      private void Start()
      {
         _animator = GetComponentInChildren<Animator>();
         _moveInputHandler = GetComponent<MoveInputHandler>();

         _moveInputHandler.forward.Execute.AddListener(e => {
            SetTargetY(1.0f);
         });

         _moveInputHandler.backward.Execute.AddListener(e => {
            SetTargetY(-1.0f);
         });

         _moveInputHandler.left.Execute.AddListener(e => {
            SetTargetX(-1.0f);
         });

         _moveInputHandler.right.Execute.AddListener(e => {
            SetTargetX(1.0f);
         });

         _moveInputHandler.OnIdle.AddListener(e => {
            SetTargetX(0.0f);
            SetTargetY(0.0f);
         });
      }

      private void Update()
      {
         _curX = Mathf.Lerp(_curX, _targetX, t);
         _curY = Mathf.Lerp(_curY, _targetY, t);

         _animator.SetFloat(X, _curX);
         _animator.SetFloat(Y, _curY);
      }

      public void SetTargetX(float x)
      {
         _targetX = x;
      } 

      public void SetTargetY(float y)
      {
         _targetY = y;
      } 


   }
}