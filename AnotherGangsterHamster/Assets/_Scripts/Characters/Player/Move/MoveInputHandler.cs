using UnityEngine;
using System.Collections.Generic;
using _Core.Commands;
using UnityEngine.Events;
using Objects;

namespace Characters.Player.Move
{
   [RequireComponent(typeof(Movement))]
   public class MoveInputHandler : MonoBehaviour
   {
      public string _path = "KeyCodes/Movements.json";

      public UnityEvent<IEventable> OnIdle
         = new UnityEvent<IEventable>();

      private Dictionary<KeyCode, Command> _moveCommands;

      // IMoveable 구체화 한 클레스
      private Movement _movement;
      
      private bool _isIdle = true;
      public bool IsIdle => _isIdle;

      // 움직임 커멘드
      public MoveForward  forward;
      public MoveBackward backward;
      public MoveLeft     left;
      public MoveRight    right;

      private void Awake()
      {
         _moveCommands  = new Dictionary<KeyCode, Command>();
         _movement      = GetComponent<Movement>();

         forward    = new MoveForward(_movement);
         backward   = new MoveBackward(_movement);
         left       = new MoveLeft(_movement);
         right      = new MoveRight(_movement);

         RemapCommands();
      }

      /// <summary>
      /// 키매핑을 초기화 후 다시 적용합니다.
      /// </summary>
      public void RemapCommands()
      {
         _moveCommands.Clear();

         MovementVO vo = Utils.JsonToVO<MovementVO>(_path);

         _moveCommands.Add((KeyCode)vo.Forward,  forward);
         _moveCommands.Add((KeyCode)vo.Backward, backward);
         _moveCommands.Add((KeyCode)vo.Left,     left);
         _moveCommands.Add((KeyCode)vo.Right,    right);
      }

      private void FixedUpdate()
      {

         foreach(KeyCode key in _moveCommands.Keys)
         {
            if(Input.GetKey(key))
            {
               _isIdle = false;
               _moveCommands[key].Execute.Invoke(null);
            }
         }

         if (_isIdle)
            OnIdle.Invoke(null);
      }
   }
}