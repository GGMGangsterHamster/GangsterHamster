using UnityEngine;

namespace Characters.Player.Move
{   
   public class MoveAnimation : MonoBehaviour
   {
      private Animator _animator;

      const string X = "X";
      const string Y = "Y";

      private void Awake()
      {
         _animator = GetComponentInChildren<Animator>();
      }

      
   }
}