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
         if(!PlayerStatus.Moveable) return;
         _delta.AddZDelta(-PlayerValues.Speed);
      }

      public void MoveForward()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.AddZDelta(PlayerValues.Speed);
      }

      public void MoveLeft()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.AddXDelta(-PlayerValues.Speed);
      }

      public void MoveRight()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.AddXDelta(PlayerValues.Speed);
      }

      private void FixedUpdate()
      {
         _rigid.MovePosition(transform.position +
                             _delta.Calculate(transform));
      }
   }
}
