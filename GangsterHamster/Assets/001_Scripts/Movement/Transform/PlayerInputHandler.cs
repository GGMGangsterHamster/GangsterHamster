using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;
using Commands.Weapon;
using Commands.Movement.Movements;
using Commands.Interaction;
using Commands.Movement.Mouse;
using Player.Mouse;

namespace Player.Movement
{

   public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
   {
      private Dictionary<KeyCode, Command> _keyDownInputDictionary;  // 키 다운

      private Dictionary<KeyCode, Command> _keyUpInputDictionary;    // 키 업

      private Dictionary<KeyCode, Command> _movementInputDictionary; // 이동 키 (FixedUpdate)

      private PlayerMovement _playerMove = null;
      private MouseMovement _mouseMove = null;
      private WeaponManagement _weapon = null;


      private void Start()
      {
         #region
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;
         #endregion

         _playerMove = FindObjectOfType<PlayerMovement>();
         _weapon = FindObjectOfType<WeaponManagement>();

         _keyDownInputDictionary = new Dictionary<KeyCode, Command>();
         _keyUpInputDictionary = new Dictionary<KeyCode, Command>();
         _movementInputDictionary = new Dictionary<KeyCode, Command>();

#if UNITY_EDITOR
         NULL.Check(_playerMove, () =>
         {
            this.enabled = false;
         });
         NULL.Check(_weapon, () =>
         {
            this.enabled = false;
         });
#endif

         #region 이동
         _movementInputDictionary.Add(KeyCode.W,
                new MoveFoward(_playerMove));   // 앞쪽

         _movementInputDictionary.Add(KeyCode.S,
                new MoveBackword(_playerMove)); // 뒤쪽

         _movementInputDictionary.Add(KeyCode.A,
                new MoveLeft(_playerMove));     // 옆쪽

         _movementInputDictionary.Add(KeyCode.D,
                new MoveRight(_playerMove));    // 오른쪽
         #endregion // 이동

         #region GetKeyDown();
         _keyDownInputDictionary.Add(KeyCode.Space,
                new Jump(_playerMove));             // 점프

         _keyDownInputDictionary.Add(KeyCode.LeftControl,
                new Crouch(_playerMove));           // 웅크리기

         _keyDownInputDictionary.Add(KeyCode.LeftShift,
                new DashStart(_playerMove));         // 대쉬 시작

         _keyDownInputDictionary.Add(KeyCode.Mouse0,
                new MouseLeft(_weapon));            // 마우스 왼쪽

         _keyDownInputDictionary.Add(KeyCode.Mouse1,
                new MouseRight(_weapon));           // 마우스 오른쪽

         _keyDownInputDictionary.Add(KeyCode.R,
                new ResetKey(_weapon));             // 무기 리셋

         _keyDownInputDictionary.Add(KeyCode.E,
                new Interact());                    // 상호작용

         _keyDownInputDictionary.Add(KeyCode.Return,
                new NextDialog());                  // 다음 다이얼로그
         #endregion // GetKeyDown();

         #region GetKeyUp();

         _keyUpInputDictionary.Add(KeyCode.LeftShift,
                new DashStop(_playerMove));         // 대쉬 중지

         #endregion // GetKeyDown();
      }

      private void FixedUpdate()
      {
         PlayerMoveDelta.Instance.ResetDelta();

         foreach (KeyCode key in _movementInputDictionary.Keys)
         { // GetKey(); for movement
            if (Input.GetKey(key))
               _movementInputDictionary[key].Execute();
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