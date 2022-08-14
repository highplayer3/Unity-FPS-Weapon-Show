using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TipsMessage;

    //让文字闪动起来
    private bool isShowTips = false;
    [SerializeField]
    private float TwinklingTime = 0.5f;
    private float m_TwinklingTimer = 0f;

    public Image GunIcon;
    public Text GunName;
    public Text GunInfo;

    public Image GrenadeIcon;
    public Text GrenadeName;
    public Text GrenadeInfo;

    public Image Crossair;
    // Start is called before the first frame update
    void Start()
    {
        TipsMessage.enabled = false;
        EventCenter.Instance.Regist("LookWeapon", OnLookWeapon);
        EventCenter.Instance.Regist("DontLookWeapon", OnDontLookWeapon);
        EventCenter.Instance.Regist("EquipmentItem", OnEquipmentItem);
        EventCenter.Instance.Regist("SetBullet", OnSetBullet);
        EventCenter.Instance.Regist("Fire", OnSetBullet);
        EventCenter.Instance.Regist("Aim", OnAim);
        EventCenter.Instance.Regist("PickUpGrenade", OnPickUpGrenade);
        EventCenter.Instance.Regist("ThrowGrenade", OnThrowGrenade);
    }

    private void OnThrowGrenade(UnityEngine.Object obj, int Param1, int Param2)
    {
        GrenadeInfo.text = string.Format("已拾取的数量:{0}", Param1);
    }

    private void OnPickUpGrenade(UnityEngine.Object obj, int Param1, int Param2)
    {
        GrenadeIcon.gameObject.SetActive(obj != null);
        GrenadeIcon.sprite = ((GrenadeData)obj).ItemImage;
        GrenadeName.text = ((GrenadeData)obj).ItemName;
        GrenadeInfo.text = string.Format("已拾取的数量:{0}", Param1);
    }

    private void OnAim(UnityEngine.Object obj, int Param1, int Param2)
    {
        if (Crossair != null)
        {
            Crossair.gameObject.SetActive(Param1 == 0);
        }
    }

    private void OnSetBullet(UnityEngine.Object obj, int Param1, int Param2)
    {
        GunInfo.text = string.Format("{0}/{1}", Param1,Param2);
    }

    private void OnEquipmentItem(UnityEngine.Object obj, int Param1, int Param2)
    {
        GunIcon.gameObject.SetActive(obj != null);
        GunData gd = ((GameObject)obj).GetComponent<Gun>().GunAttr;
        GunIcon.sprite = gd.ItemImage;
        GunName.text = gd.ItemName;
       
    }

    private void OnDontLookWeapon(UnityEngine.Object obj, int Param1, int Param2)
    {
        isShowTips = false;
        TipsMessage.enabled = false;
    }

    private void OnLookWeapon(UnityEngine.Object obj, int Param1, int Param2)
    {
        isShowTips = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isShowTips)
        {
            m_TwinklingTimer += Time.deltaTime;
            if (m_TwinklingTimer > TwinklingTime)
            {
                TipsMessage.enabled = !TipsMessage.enabled;
                m_TwinklingTimer = 0f;
                
            }
        }
    }
}
