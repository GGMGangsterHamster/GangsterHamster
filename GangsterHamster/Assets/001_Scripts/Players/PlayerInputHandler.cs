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
        private Dictionary<KeyCode, Command> _toggleInputDictionary = new Dictionary<KeyCode, Command>();
        private Dictionary<KeyCode, Command> _pushInputDictionary = new Dictionary<KeyCode, Command>();
        private PlayerMovement _playerMove = null;
        private WeaponManagement _weapon = null;

        private MouseX _mouseX;
        private MouseY _mouseY;


        // 특수한 입력이 필요한 키들
        Dash _dash;

        private void Start()
        {
            // �׽�Ʈ �� �ƹ����� ������ ��
            #region
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            #endregion


            _playerMove = FindObjectOfType<PlayerMovement>();
            _weapon = FindObjectOfType<WeaponManagement>();

            if (_playerMove == null)
            {
                Logger.Log("PlayerInputHandler > _playerMove is null", LogLevel.Fatal);
            }

            _pushInputDictionary.Add(KeyCode.W, new MoveFoward(_playerMove));
            _pushInputDictionary.Add(KeyCode.S, new MoveBackword(_playerMove));
            _pushInputDictionary.Add(KeyCode.A, new MoveLeft(_playerMove));
            _pushInputDictionary.Add(KeyCode.D, new MoveRight(_playerMove));
            _toggleInputDictionary.Add(KeyCode.Space, new Jump(_playerMove));
            _toggleInputDictionary.Add(KeyCode.LeftControl, new Crouch(_playerMove));
            _toggleInputDictionary.Add(KeyCode.E, new Interact());

            _toggleInputDictionary.Add(KeyCode.Mouse0, new MouseLeft(_weapon));
            _toggleInputDictionary.Add(KeyCode.Mouse1, new MouseRight(_weapon));
            _toggleInputDictionary.Add(KeyCode.R, new ResetKey(_weapon));

            _mouseX = new MouseX(_playerMove);
            _mouseY = new MouseY(_playerMove);
            _dash = new Dash(_playerMove);
        }

        private void Update()
        {
            foreach(KeyCode key in _pushInputDictionary.Keys) { // GetKey();
                if(Input.GetKey(key))
                    _pushInputDictionary[key].Execute();
            }

            foreach (KeyCode key in _toggleInputDictionary.Keys) { // GetKeyDown();
                if (Input.GetKeyDown(key))
                    _toggleInputDictionary[key].Execute();
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