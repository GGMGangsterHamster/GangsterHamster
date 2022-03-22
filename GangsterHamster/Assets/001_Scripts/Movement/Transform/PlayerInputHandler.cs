using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;
using Commands.Weapon;
using Commands.Movement.Movements;
using Commands.Interaction;
using Commands.Movement.Mouse;
using Player.Mouse;
using Player.Status;

namespace Player.Movement
{
   public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
   {
      private Dictionary<KeyCode, Command> _keyDownInputDictionary;  // 키 다운

      private Dictionary<KeyCode, Command> _keyUpInputDictionary;    // 키 업

      private Dictionary<KeyCode, Command> _movementInputDictionary; // 이동 키 (FixedUpdate)

      private PlayerMovement _playerMove = null;
      private PlayerMovement PlayerMove
      {
         get
         {
            if (_playerMove == null)
            {
               _playerMove = FindObjectOfType<PlayerMovement>();

            }
            return _playerMove;
         }
      }

      private WeaponManagement _weapon = null;
      private WeaponManagement Weapon
      {
         get
         {
            if (_weapon == null)
            {
               _weapon = FindObjectOfType<WeaponManagement>();

            }
            return _weapon;
         }   
      }


      private void Start()
      {
         #region // Cursor lock
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;
         #endregion

         _keyDownInputDictionary  = new Dictionary<KeyCode, Command>();
         _keyUpInputDictionary    = new Dictionary<KeyCode, Command>();
         _movementInputDictionary = new Dictionary<KeyCode, Command>();

         #region 이동
         _movementInputDictionary.Add(KeyCode.W,
                new MoveFoward(PlayerMove));   // 앞쪽

         _movementInputDictionary.Add(KeyCode.S,
                new MoveBackword(PlayerMove)); // 뒤쪽

         _movementInputDictionary.Add(KeyCode.A,
                new MoveLeft(PlayerMove));     // 옆쪽

         _movementInputDictionary.Add(KeyCode.D,
                new MoveRight(PlayerMove));    // 오른쪽
         #endregion // 이동

         #region GetKeyDown();
         _keyDownInputDictionary.Add(KeyCode.Space,
                new Jump(PlayerMove));             // 점프

         _keyDownInputDictionary.Add(KeyCode.LeftControl,
                new Crouch(PlayerMove));           // 웅크리기

         _keyDownInputDictionary.Add(KeyCode.LeftShift,
                new DashStart(PlayerMove));         // 대쉬 시작

         _keyDownInputDictionary.Add(KeyCode.Mouse0,
                new MouseLeft(Weapon));            // 마우스 왼쪽

         _keyDownInputDictionary.Add(KeyCode.Mouse1,
                new MouseRight(Weapon));           // 마우스 오른쪽

         _keyDownInputDictionary.Add(KeyCode.R,
                new ResetKey(Weapon));             // 무기 리셋

         _keyDownInputDictionary.Add(KeyCode.E,
                new Interact());                    // 상호작용

         _keyDownInputDictionary.Add(KeyCode.Return,
                new NextDialog());                  // 다음 다이얼로그
         #endregion // GetKeyDown();

         #region GetKeyUp();

         _keyUpInputDictionary.Add(KeyCode.LeftShift,
                new DashStop(PlayerMove));         // 대쉬 중지

         #endregion // GetKeyDown();
      }

      private void FixedUpdate()
      {
         PlayerMoveDelta.Instance.ResetDelta();

         // GetKey(); for movement

         foreach (KeyCode key in _movementInputDictionary.Keys)
         {
            if (Input.GetKey(key))
            {
               PlayerStatus.IsMoving = true;
               _movementInputDictionary[key].Execute();
            }
         }

      }

      private void Update()
      {
         foreach (KeyCode key in _keyDownInputDictionary.Keys)
         {
            if (Input.GetKeyDown(key))
               _keyDownInputDictionary[key].Execute();
         }

         foreach (KeyCode key in _keyUpInputDictionary.Keys)
         {
            if (Input.GetKeyUp(key))
               _keyUpInputDictionary[key].Execute();
         }

      }
   }
}