using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSensor : MonoBehaviour
{
    public Func<bool> requirement; // 투명해지는 조건

    [SerializeField] private float _onValue;

    private AlphaShader _alphaShader;

    private void Awake()
    {
        _alphaShader = GetComponent<AlphaShader>();

        requirement = () => false;
    }

    private void Update()
    {
        if(requirement())
        {
            _alphaShader.Alpha = _onValue;
        }
        else
        {
            _alphaShader.Alpha = 1;
        }
    }
}
