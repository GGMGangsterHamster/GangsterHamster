using Characters.Player;
using UnityEngine;

namespace Characters.Player.Move
{
   [RequireComponent(typeof(Rigidbody),
                     typeof(MoveDelta))]
   public class Movement : MonoBehaviour, IMoveable
   {
      private MoveDelta _delta;
      private Rigidbody _rigid;

      private void Awake()
      {
         _rigid = GetComponent<Rigidbody>();
         _delta = GetComponent<MoveDelta>();
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
