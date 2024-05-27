using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshThingh : MonoBehaviour
{
    private MeshRenderer[] _meshes;
    
    private void Awake()
    {
        _meshes = GetComponentsInChildren<MeshRenderer>();
    }

    public void Show()
    {
        foreach (MeshRenderer mesh in _meshes)
        {
            mesh.enabled = true;
        }
    }
    
    public void Hide()
    {
        foreach (MeshRenderer mesh in _meshes)
        {
            mesh.enabled = false;
        }
    }
}
