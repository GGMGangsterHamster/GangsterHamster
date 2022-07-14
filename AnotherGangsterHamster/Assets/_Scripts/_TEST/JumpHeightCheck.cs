using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 무기들의 테스트를 위해서 만들어 놓은 스크립트
/// </summary>
public class JumpHeightCheck : MonoBehaviour
{
    public Text maxHeightText;

    private Transform _playerBaseTransform;
    private float _playerStartHeight = 0.0f;
    private float _maxHeight = 0.0f;
    private float _currentHeight = 0.0f;

    private bool _checkStart = false;
    private Transform PlayerBaseTransform
    {
        get
        {
            if (_playerBaseTransform == null)
            {
                _playerBaseTransform = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
            }

            return _playerBaseTransform;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            _playerStartHeight = PlayerBaseTransform.position.y;
            _checkStart = true;
            maxHeightText.color = Color.green;
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            _checkStart = false;
            maxHeightText.text = "MaxHeight : " + 0;
            _maxHeight = 0;
            maxHeightText.color = Color.red;
        }
    }

    private void FixedUpdate()
    {
        if (_checkStart)
        {
            _currentHeight = PlayerBaseTransform.position.y - _playerStartHeight;

            if (_currentHeight > _maxHeight)
            {
                _maxHeight = _currentHeight;
                maxHeightText.text = "MaxHeight : " + _maxHeight;
            }
        }
    }
}
