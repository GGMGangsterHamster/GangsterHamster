using UnityEngine;
using System.Collections.Generic;
using _Core.Commands;

namespace Characters.Player.Move
{
   [RequireComponent(typeof(Movement))]
   public class MoveInputHandler : MonoBehaviour
   {
      public string _path = "KeyCodes/Movements.json";

      private Dictionary<KeyCode, Command> _moveCommands;

      // IMoveable 구체화 한 클레스
      private Movement _movement;

      // 움직임 커멘드
      private MoveForward  _forward;
      private MoveBackward _backward;
      private MoveLeft     _left;
      private MoveRight    _right;

      private void Awake()
      {
         _moveCommands  = new Dictionary<KeyCode, Command>();
         _movement      = GetComponent<Movement>();

         _forward    = new MoveForward(_movement);
         _backward   = new MoveBackward(_movement);
         _left       = new MoveLeft(_movement);
         _right      = new MoveRight(_movement);

         RemapCommands();
      }

      /// <summary>
      /// 키매핑을 초기화 후 다시 적용합니다.
      /// </summary>
      public void RemapCommands()
      {
         _moveCommands.Clear();

         MovementVO vo = Utils.JsonToVO<MovementVO>(_path);

         _moveCommands.Add((KeyCode)vo.Forward,  _forward);
         _moveCommands.Add((KeyCode)vo.Backward, _backward);
         _moveCommands.Add((KeyCode)vo.Left,     _left);
         _moveCommands.Add((KeyCode)vo.Right,    _right);
      }

      private void FixedUpdate()
      {
         foreach(KeyCode key in _moveCommands.Keys)
         {
            if(Input.GetKey(key))
               _moveCommands[key].Execute.Invoke(null);
         }
      }
   }
}