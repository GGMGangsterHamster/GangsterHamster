using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaShader : MonoBehaviour
{
    private Material _alphaMat;

    public float Alpha
    {
        get
        {
            return _alphaMat.color.a;
        }
        set
        {
            _alphaMat.color = new Color(_alphaMat.color.r,
                                        _alphaMat.color.g,
                                        _alphaMat.color.b,
                                        value
            );
        }
    }

    private void Awake()
    {
        Material mat = GetComponent<MeshRenderer>().material;

        _alphaMat = mat;

        if(_alphaMat == null)
        {
            Debug.LogError("���� Material�� �����");
            Time.timeScale = 0;
        }
    }
}
