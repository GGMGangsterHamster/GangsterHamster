using UnityEngine;

namespace Characters.Player.Move
{   
   public class MoveAnimation : MonoBehaviour
   {
      public Animator animator;
      private MoveInputHandler _moveInputHandler;

      const string X = "X";
      const string Y = "Y";

      private void Start()
      {
         _moveInputHandler = GetComponent<MoveInputHandler>();

         _moveInputHandler.forward.Execute.AddListener(e => {
            animator.SetFloat(Y, 1.0f);
         });

         _moveInputHandler.backward.Execute.AddListener(e => {
            animator.SetFloat(Y, -1.0f);
         });

         _moveInputHandler.left.Execute.AddListener(e => {
            animator.SetFloat(X, -1.0f);
         });

         _moveInputHandler.right.Execute.AddListener(e => {
            animator.SetFloat(X, 1.0f);
         });

         _moveInputHandler.OnIdle.AddListener(e => {
            animator.SetFloat(X, 0.0f);
            animator.SetFloat(Y, 0.0f);
         });

         // TODO: 카메라 보정과 Lerp
      }


   }
}