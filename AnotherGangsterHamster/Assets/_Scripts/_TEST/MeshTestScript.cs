using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTestScript : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mesh = _meshFilter.mesh;
    }
}
