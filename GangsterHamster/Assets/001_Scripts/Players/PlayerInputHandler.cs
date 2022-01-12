using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commands;
using Commands.Movement;
using Commands.Movement.Movements;

public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
{
    private Dictionary<KeyCode, Command> _inputDictionary = new Dictionary<KeyCode, Command>();
    private PlayerMovement _playerMove = null;

    // TODO : Keymaps

    private void Start()
    {
        _playerMove = FindObjectOfType<PlayerMovement>();
        if(_playerMove == null) {
            Log.Debug.Log("PlayerInputHandler > _playerMove is null", Log.LogLevel.Fatal);
        }

        _inputDictionary.Add(KeyCode.W, new MoveFoward(_playerMove));
        _inputDictionary.Add(KeyCode.S, new MoveBackword(_playerMove));
        _inputDictionary.Add(KeyCode.A, new MoveLeft(_playerMove));
        _inputDictionary.Add(KeyCode.D, new MoveRight(_playerMove));
        _inputDictionary.Add(KeyCode.Space, new Jump(_playerMove));
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

        float x = Input.GetAxis("Mouse X");
        float y = -Input.GetAxis("Mouse Y");
        _playerMove.transform.eulerAngles += new Vector3(y, x, 0);
    }
}
