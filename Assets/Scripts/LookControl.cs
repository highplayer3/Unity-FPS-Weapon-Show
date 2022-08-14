using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookControl : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 200f;
    [SerializeField]
    private float minAngle=-85f;
    [SerializeField]
    private float maxAngle=85f;
    private float mouseX;
    private float mouseY;
    private float m_RotationY;
    public Transform player;
    [SerializeField]
    private float PickUpDistance = 2f;
    [SerializeField]
    private LayerMask PickUpMask;

    private GameObject hasTarget = null;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventCenter.Instance.Regist("Fire", OnFire);
        
    }
    //实现后坐力
    private void OnFire(UnityEngine.Object obj, int Param1, int Param2)
    {
        m_RotationY -= ((GunData)obj).Recoil / 10;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;
        m_RotationY -= mouseY;//为什么是减,因为鼠标向上时mouseY>0,但向上旋转时，绕X轴的角度<0
        m_RotationY = Mathf.Clamp(m_RotationY, minAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(m_RotationY, 0, 0);
        if (player != null)
        {
            player.Rotate(player.up, mouseX);
        }
        LookItem_Update();
        PickUp_Update();
    }
    private void PickUp_Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&hasTarget!=null)
        {
            Debug.Log(hasTarget);
            EventCenter.Instance.Trigger("PickUpItem", hasTarget, 0, 0);
        }
    }
    /// <summary>
    /// 角色是否指向武器
    /// </summary>
    private void LookItem_Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit, PickUpDistance,PickUpMask)) 
        {
            if (hasTarget != hit.collider.gameObject)
            {
                hasTarget = hit.collider.gameObject;
                EventCenter.Instance.Trigger("LookWeapon", hasTarget, 0, 0);
            }
        }
        else
        {
            if (hasTarget!=null)
            {
                EventCenter.Instance.Trigger("DontLookWeapon", hasTarget, 0, 0);
                hasTarget = null;
            }
               
        }
    }
}
