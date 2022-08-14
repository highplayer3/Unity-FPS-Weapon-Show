using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    public Transform ArmPos;
    public Camera EyesCamera;
    public Camera SceneCamera;
    [HideInInspector]
    public int grenadeNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.Regist("PickUpItem", OnPickUpItem);
        EventCenter.Instance.Regist("ThrowGrenade", OnThrowGrenade);
    }

    private void OnThrowGrenade(Object obj, int Param1, int Param2)
    {
        grenadeNumber = Param1;
    }

    private void OnPickUpItem(Object obj, int Param1, int Param2)
    {
        //如果拾取的是手雷
        if (((GameObject)obj).CompareTag("Grenade"))
        {
            ItemAttr g_attr=((GameObject)obj).GetComponent<ItemAttr>();
            if (g_attr != null)
            {
                GrenadeData m_data = g_attr.AttrData as GrenadeData;
                if (grenadeNumber < m_data.maxNumber)
                {
                    grenadeNumber++;
                    EventCenter.Instance.Trigger("PickUpGrenade", m_data, grenadeNumber, 0);
                    Debug.Log(grenadeNumber);
                    //EventCenter.Instance.Trigger("TellThePlayer", m_data, 0, 0);
                }
                else
                {
                    Debug.Log("手雷拾取已到最大限制");
                }
            }
            return;
        }
        //和眼睛保持同步
        if (SceneCamera != null)
        {
            SceneCamera.transform.SetParent(EyesCamera.transform);
        }
        while (ArmPos.childCount > 0)
        {
            GameObject t = ArmPos.GetChild(0).gameObject;
            if (t != null)
            {
                DestroyImmediate(t);
            }
        }
        GameObject item = (GameObject)obj;
        ItemAttr attr = item.GetComponent<ItemAttr>();
        if (attr != null)
        {
            //添加装备给人物
            GameObject equipItem = GameObject.Instantiate(attr.AttrData.ItemEqquipmentPrefab,ArmPos);
            Gun g = equipItem.GetComponent<Gun>();
            if (g != null)
            {
                g.Init((GunData)attr.AttrData,EyesCamera,SceneCamera,this.gameObject);
            }
            EventCenter.Instance.Trigger("EquipmentItem", equipItem, 0, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
