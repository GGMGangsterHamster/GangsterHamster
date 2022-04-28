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
         _delta.SubZDelta();
      }

      public void MoveForward()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.AddZDelta();
      }

      public void MoveLeft()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.SubXDelta();
      }

      public void MoveRight()
      {
         if(!PlayerStatus.Moveable) return;
         _delta.AddXDelta();
      }

      private void FixedUpdate() // TOOD: 루트모션 넣으면 바꿔야 함
      {
         // transform.Translate(_delta.Calculate(transform, PlayerValues.Speed));
         // _rigid.MovePosition(transform.position +
         //                     _delta.Calculate(transform, PlayerValues.Speed));

         transform.Translate(_delta.Calculate(transform, PlayerValues.Speed, true));
      }
   }
}
