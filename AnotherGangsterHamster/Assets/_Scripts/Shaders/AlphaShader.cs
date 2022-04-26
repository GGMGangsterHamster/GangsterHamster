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
            return _alphaMat.GetFloat("alpha");
        }
        set
        {
            _alphaMat.SetFloat("alpha", value);
        }
    }

    private void Awake()
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;

        foreach(Material mat in mats)
        {
            if (mat.name == "AlphaMat (Instance)")
            {
                _alphaMat = mat;
                break;
            }
        }

        if(_alphaMat == null)
        {
            Debug.LogError("알파 Material이 없어요");
        }
    }
}
