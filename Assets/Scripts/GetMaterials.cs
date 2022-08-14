using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 获取角色所在地方的材质
/// </summary>
public class GetMaterials : MonoBehaviour
{
    private Material m_material;
    public Material m_Material
    {
        get
        {
            return m_material;
        }
        private set { }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Plane"))
        {
            //获取默认材质，若有多个材质球，则采用数组的形式获取对应材质球
            m_Material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
            if (m_Material != null)
            {
                
            }
            
        }
    }
}
