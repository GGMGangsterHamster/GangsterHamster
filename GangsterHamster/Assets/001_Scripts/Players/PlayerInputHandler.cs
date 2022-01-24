using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commands;
using Commands.Movement;
using Commands.Movement.Movements;
using Commands.Weapon;

public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
{
    private Dictionary<KeyCode, Command> _inputDictionary = new Dictionary<KeyCode, Command>();
    private PlayerMovement _playerMove = null;
    private WeaponManagement _weapon = null;

    private MouseX mouseX;
    private MouseY mouseY;


    // TODO: Keymaps

    private void Start()
    {
        // 테스트 용 아무때나 지워도 댐
        #region
        Cursor.visible = false;
        #endregion


        _playerMove = FindObjectOfType<PlayerMovement>();
        _weapon = FindObjectOfType<WeaponManagement>();

        if (_playerMove == null) {
            Log.Debug.Log("PlayerInputHandler > _playerMove is null", Log.LogLevel.Fatal);
        }

        _inputDictionary.Add(KeyCode.W, new MoveFoward(_playerMove));
        _inputDictionary.Add(KeyCode.S, new MoveBackword(_playerMove));
        _inputDictionary.Add(KeyCode.A, new MoveLeft(_playerMove));
        _inputDictionary.Add(KeyCode.D, new MoveRight(_playerMove));
        _inputDictionary.Add(KeyCode.Space, new Jump(_playerMove));

        _inputDictionary.Add(KeyCode.Mouse0, new MouseLeft(_weapon));
        _inputDictionary.Add(KeyCode.Mouse1, new MouseRight(_weapon));
        _inputDictionary.Add(KeyCode.R, new ResetKey(_weapon));

        mouseX = new MouseX(_playerMove);
        mouseY = new MouseY(_playerMove);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W)) {
            _inputDictionary[KeyCode.W].Execute();
        }
        if (Input.GetKey(KeyCode.S)) {
            _inputDictionary[KeyCode.S].Execute();
        }
        if (Input.GetKey(KeyCode.A)) {
            _inputDictionary[KeyCode.A].Execute();
        }
        if (Input.GetKey(KeyCode.D)) {
            _inputDictionary[KeyCode.D].Execute();
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            _inputDictionary[KeyCode.Space].Execute();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _inputDictionary[KeyCode.Mouse0].Execute();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _inputDictionary[KeyCode.Mouse1].Execute();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _inputDictionary[KeyCode.R].Execute();
        }


        //float x = Input.GetAxis("Mouse X");
        //float y = -Input.GetAxis("Mouse Y");
        //_playerMove.transform.eulerAngles += new Vector3(y, x, 0);

        mouseX.Execute();
        mouseY.Execute();
    }
}
