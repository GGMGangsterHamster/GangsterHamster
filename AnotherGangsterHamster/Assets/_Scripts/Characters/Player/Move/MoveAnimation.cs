using UnityEngine;

namespace Characters.Player.Move
{   
   public class MoveAnimation : MonoBehaviour
   {
      private Animator _animator;
      private MoveInputHandler _moveInputHandler;

      const string X = "X";
      const string Y = "Y";

      private void Awake()
      {
         _animator = GetComponentInChildren<Animator>();
         _moveInputHandler = GetComponent<MoveInputHandler>();


         _moveInputHandler.forward.Execute.AddListener(e => {
            _animator.SetFloat(Y, 1.0f);
         });

         _moveInputHandler.backward.Execute.AddListener(e => {
            _animator.SetFloat(Y, -1.0f);
         });

         _moveInputHandler.left.Execute.AddListener(e => {
            _animator.SetFloat(X, -1.0f);
         });

         _moveInputHandler.right.Execute.AddListener(e => {
            _animator.SetFloat(X, 1.0f);
         });
      }


   }
}