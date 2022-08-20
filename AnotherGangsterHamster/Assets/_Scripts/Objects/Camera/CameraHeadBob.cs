using Characters.Player.Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    private float _defaultHeight;
    private float _maxHeight;
    private float _minHeight;
    private bool _isUP = true;

    private MoveInputHandler _moveInputHandler;

    void Start()
    {
        _defaultHeight = transform.position.y;
        _maxHeight = _defaultHeight + 0.05f;
        _minHeight = _defaultHeight - 0.05f;

        _moveInputHandler = GetComponentInParent<MoveInputHandler>();
    }

    void Update()
    {
        if (!_moveInputHandler.IsIdle)
        {
            HeadBob();
        }
    }

    public void HeadBob()
    {
        if (transform.position.y >= _maxHeight)
        {
            _isUP = false;
        }
        else if(transform.position.y <= _minHeight)
        {
            _isUP = true;
        }

        if (_isUP)
        {
            transform.position += new Vector3(0, Time.deltaTime * 0.5f);
        }
        else
        {
            transform.position -= new Vector3(0, Time.deltaTime * 0.5f);
        }
    }
}
