using Chararcters.Player;
using UnityEngine;

namespace Characters.Player.Move
{
   [RequireComponent(typeof(Rigidbody),
                     typeof(PlayerMoveDelta))]
   public class PlayerMovement : MonoBehaviour, IMoveable
   {
      private PlayerMoveDelta _delta;
      private Rigidbody _rigid;

      private void Awake()
      {
         _rigid = GetComponent<Rigidbody>();
         _delta = GetComponent<PlayerMoveDelta>();
      }

      public void MoveBackward()
      {
         _delta.AddZDelta(-PlayerValues.Speed);
      }

      public void MoveForward()
      {
         _delta.AddZDelta(PlayerValues.Speed);
      }

      public void MoveLeft()
      {
         _delta.AddXDelta(-PlayerValues.Speed);
      }

      public void MoveRight()
      {
         _delta.AddXDelta(PlayerValues.Speed);
      }

      private void FixedUpdate()
      {
         _rigid.MovePosition(transform.position +
                             _delta.Calculate(transform));
      }
   }
}
