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
        private Dictionary<KeyCode, Command> _keydownInputDictionary = new Dictionary<KeyCode, Command>(); // 키 다운 입력
        private Dictionary<KeyCode, Command> _movementInputDictionary = new Dictionary<KeyCode, Command>(); // 이동 키 입력 (FixedUpdate)

        private PlayerMovement _playerMove = null;
        private WeaponManagement _weapon = null;



        // 특수한 입력이 필요한 키들
        private Dash _dash;
        private MouseX _mouseX;
        private MouseY _mouseY;

        private void Start()
        {
            // �׽�Ʈ �� �ƹ����� ������ ��
            #region
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            #endregion

            _playerMove = FindObjectOfType<PlayerMovement>();
            _weapon = FindObjectOfType<WeaponManagement>();

            NULL.Check(_playerMove, () => {
                this.enabled = false;
            });
            NULL.Check(_weapon, () => {
                this.enabled = false;
            });
            #region GetKey();
            _movementInputDictionary.Add(KeyCode.W, new MoveFoward(_playerMove));
            _movementInputDictionary.Add(KeyCode.S, new MoveBackword(_playerMove));
            _movementInputDictionary.Add(KeyCode.A, new MoveLeft(_playerMove));
            _movementInputDictionary.Add(KeyCode.D, new MoveRight(_playerMove));
            #endregion // GetKey();

            #region GetKeyDown();
            _keydownInputDictionary.Add(KeyCode.Space, new Jump(_playerMove));
            _keydownInputDictionary.Add(KeyCode.LeftControl, new Crouch(_playerMove));
            _keydownInputDictionary.Add(KeyCode.Mouse0, new MouseLeft(_weapon));
            _keydownInputDictionary.Add(KeyCode.Mouse1, new MouseRight(_weapon));
            _keydownInputDictionary.Add(KeyCode.R, new ResetKey(_weapon));

            _keydownInputDictionary.Add(KeyCode.E, new Interact());
            _keydownInputDictionary.Add(KeyCode.Return, new NextDialog());
            #endregion // GetKeyDown();

            #region Special inputs
            _mouseX = new MouseX(_playerMove);
            _mouseY = new MouseY(_playerMove);
            _dash = new Dash(_playerMove);
            #endregion // Special inputs
        }

        private void FixedUpdate() {
            PlayerMoveDelta.Instance.ResetDelta();

            foreach (KeyCode key in _movementInputDictionary.Keys) { // GetKey(); for movement
                if (Input.GetKey(key))
                    _movementInputDictionary[key].Execute();
            }
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

            #region Move
            Vector3 dir = PlayerMoveDelta.Instance.GetDelta();
            dir.z = dir.y;
            dir.y = 0;
            GameManager.Instance.player.transform.Translate(dir * Time.deltaTime);
            #endregion // Move




            _mouseX.Execute();
            _mouseY.Execute();
        }
    }
}