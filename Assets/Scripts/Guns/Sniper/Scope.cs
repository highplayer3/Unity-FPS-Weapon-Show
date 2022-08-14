using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    
    public Camera ScopeCamera;
    public GameObject ScopeRenderMesh;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.Regist("Aim", OnAim);
    }

    private void OnAim(UnityEngine.Object obj, int Param1, int Param2)
    {
        if (ScopeCamera != null)
        {
            ScopeCamera.gameObject.SetActive(Param1 == 1);
        }
        if (ScopeRenderMesh != null)
        {
            ScopeRenderMesh.SetActive(Param1 == 1);
        }
    }

    
}
