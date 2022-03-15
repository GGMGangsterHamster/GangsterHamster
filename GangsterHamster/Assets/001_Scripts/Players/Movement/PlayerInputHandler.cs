using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;
using Commands.Weapon;
using Commands.Movement.Movements;
using Commands.Interaction;

namespace Player.Movement
{

    public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
    {
        private Dictionary<KeyCode, Command> _keydownInputDictionary = new Dictionary<KeyCode, Command>(); // 키 다운
        private Dictionary<KeyCode, Command> _movementInputDictionary = new Dictionary<KeyCode, Command>(); // 이동 키 (FixedUpdate)

        private PlayerMovement _playerMove = null;
        private WeaponManagement _weapon = null;



        // 특수한 입력이 필요한 키들
        private Dash _dash;
        private MouseX _mouseX;
        private MouseY _mouseY;

        private void Start()
        {
            #region
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            #endregion

            _playerMove = FindObjectOfType<PlayerMovement>();
            _weapon = FindObjectOfType<WeaponManagement>();

            #if UNITY_EDITOR
            NULL.Check(_playerMove, () => {
                this.enabled = false;
            });
            NULL.Check(_weapon, () => {
                this.enabled = false;
            });
            #endif

            #region 이동
            _movementInputDictionary.Add(KeyCode.W, new MoveFoward(_playerMove));   // 앞쪽
            _movementInputDictionary.Add(KeyCode.S, new MoveBackword(_playerMove)); // 뒤쪽
            _movementInputDictionary.Add(KeyCode.A, new MoveLeft(_playerMove));     // 옆쪽
            _movementInputDictionary.Add(KeyCode.D, new MoveRight(_playerMove));    // 오른쪽
            #endregion // 이동

            #region GetKeyDown();
            _keydownInputDictionary.Add(KeyCode.Space, new Jump(_playerMove));          // 점프
            _keydownInputDictionary.Add(KeyCode.LeftControl, new Crouch(_playerMove));  // 웅크리기
            _keydownInputDictionary.Add(KeyCode.Mouse0, new MouseLeft(_weapon));        // 마우스 왼쪽
            _keydownInputDictionary.Add(KeyCode.Mouse1, new MouseRight(_weapon));       // 마우스 오른쪽
            _keydownInputDictionary.Add(KeyCode.R, new ResetKey(_weapon));              // 무기 리셋

            _keydownInputDictionary.Add(KeyCode.E, new Interact());         // 상호작용
            _keydownInputDictionary.Add(KeyCode.Return, new NextDialog());  // 다음 다이얼로그
            #endregion // GetKeyDown();

            #region Special inputs
            _mouseX = new MouseX(_playerMove);  // 마우스 X
            _mouseY = new MouseY(_playerMove);  // 마우스 Y
            _dash = new Dash(_playerMove);      // 대쉬 (Shift)
            #endregion // Special inputs
        }

        private void FixedUpdate() {
            PlayerMoveDelta.Instance.ResetDelta();

            foreach (KeyCode key in _movementInputDictionary.Keys) { // GetKey(); for movement
                if (Input.GetKey(key))
                    _movementInputDictionary[key].Execute();
            }

            #region Move
            Vector3 dir = PlayerMoveDelta.Instance.GetDelta();
            dir.z = dir.y;
            dir.y = 0;

            Rigidbody rigid = GameManager.Instance.PlayerRigid;

            rigid.velocity = new Vector3(dir.x + rigid.velocity.x, rigid.velocity.y, dir.z + rigid.velocity.z);
            #endregion // Move
        }
         
        private void Update()
        {
            foreach (KeyCode key in _keydownInputDictionary.Keys) { // GetKeyDown();
                if (Input.GetKeyDown(key))
                    _keydownInputDictionary[key].Execute();
            }

            #region Shift (dash)
            if(Input.GetKeyDown(KeyCode.LeftShift))
                _dash.Execute();
            
            if(Input.GetKeyUp(KeyCode.LeftShift))
                _dash.Execute();
            #endregion // Shift (dash)

            

            _mouseX.Execute();
            _mouseY.Execute();
        }
    }
}