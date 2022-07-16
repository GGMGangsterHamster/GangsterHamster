using UnityEngine;

namespace Characters.Player.Actions
{
   public class ActionAnimation : MonoBehaviour
   {
      readonly int JUMP = Animator.StringToHash("Jump");

      private Animator _animator;
      private ActionInputHandler _actionInputHandler;


      private void Start()
      {
         _animator = GetComponentInChildren<Animator>();
         _actionInputHandler = GetComponent<ActionInputHandler>();


         _actionInputHandler.jump.Execute.AddListener(e => {
            _animator.SetTrigger(JUMP);
         });
      }
   }
}